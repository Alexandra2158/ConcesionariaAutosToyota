using ConcesionariaAutosToyota.Trade.Application.Interfaces;
using ConcesionariaAutosToyota.Trade.Application.UseCases.Commands.Auth;
using ConcesionariaAutosToyota.Trade.Application.UseCases.Commands.Order;
using ConcesionariaAutosToyota.Trade.Application.UseCases.Commands.Stock;
using ConcesionariaAutosToyota.Trade.Application.UseCases.Queries.Order;
using ConcesionariaAutosToyota.Trade.Application.UseCases.Queries.Stock;
using ConcesionariaAutosToyota.Trade.Infrastructure.Messaging;
using ConcesionariaAutosToyota.Trade.Infrastructure.Persistence.Contexts;
using ConcesionariaAutosToyota.Trade.Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Microsoft.OpenApi.Any;

var builder = WebApplication.CreateBuilder(args);

// ── BASE DE DATOS SQL SERVER ──────────────────────────────────────────────────
builder.Services.AddDbContext<ApplicationDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        sql => sql.EnableRetryOnFailure(3)));

// ── REPOSITORIOS (Inyección de dependencias) ──────────────────────────────────
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IStockRepository, StockRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IEventPublisher, RabbitMqEventPublisher>();

// ── HANDLERS CQRS ─────────────────────────────────────────────────────────────
builder.Services.AddScoped<LoginUserHandler>();
builder.Services.AddScoped<CrearStockHandler>();
builder.Services.AddScoped<CrearOrderHandler>();
builder.Services.AddScoped<GetStocksHandler>();
builder.Services.AddScoped<GetOrdersHandler>();

// ── JWT AUTENTICACIÓN + AUTORIZACIÓN ─────────────────────────────────────────
var jwtSection = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSection["SecretKey"]!;
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSection["Issuer"],
            ValidAudience = jwtSection["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        };
    });
builder.Services.AddAuthorization();

// ── SWAGGER con soporte JWT ───────────────────────────────────────────────────
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ConcesionariaAutosToyota.Trade API",
        Version = "v1",
        Description = "API REST para gestión del concesionario Toyota — .NET 8 + CQRS + JWT"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingrese: Bearer {token}"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme { Reference = new OpenApiReference
                { Type = ReferenceType.SecurityScheme, Id = "Bearer" }},
            Array.Empty<string>()
        }
    });
});

builder.Services.AddControllers();

var app = builder.Build();

// ── MIGRACIONES AUTOMÁTICAS ───────────────────────────────────────────────────
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

// ── MIDDLEWARE ────────────────────────────────────────────────────────────────
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Toyota API v1"));

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
