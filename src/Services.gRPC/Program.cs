using ConcesionariaAutosToyota.Trade.Application.Interfaces;
using ConcesionariaAutosToyota.Trade.Infrastructure.Persistence.Contexts;
using ConcesionariaAutosToyota.Trade.Infrastructure.Persistence.Repositories;
using ConcesionariaAutosToyota.Trade.Services.gRPC.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        sql => sql.EnableRetryOnFailure(3)));

builder.Services.AddScoped<IStockRepository, StockRepository>();
builder.Services.AddGrpc();

var app = builder.Build();

app.MapGrpcService<StockGrpcServiceImpl>();
app.MapGet("/", () => "gRPC ConcesionariaAutosToyota — usa un cliente gRPC para conectarse.");

app.Run();
