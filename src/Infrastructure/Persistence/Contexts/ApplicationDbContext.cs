using ConcesionariaAutosToyota.Trade.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ConcesionariaAutosToyota.Trade.Infrastructure.Persistence.Contexts;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Stock> Stocks { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Stock>(e =>
        {
            e.HasKey(s => s.Id);
            e.Property(s => s.Modelo).IsRequired().HasMaxLength(100);
            e.Property(s => s.NumeroVIN).IsRequired().HasMaxLength(17);
            e.Property(s => s.Precio).HasColumnType("decimal(18,2)");
        });

        modelBuilder.Entity<Order>(e =>
        {
            e.HasKey(o => o.Id);
            e.Property(o => o.Total).HasColumnType("decimal(18,2)");
            e.HasOne(o => o.Customer).WithMany().HasForeignKey(o => o.CustomerId);
            e.HasOne(o => o.Stock).WithMany().HasForeignKey(o => o.StockId);
        });

        modelBuilder.Entity<Customer>(e =>
        {
            e.HasKey(c => c.Id);
            e.Property(c => c.Email).IsRequired().HasMaxLength(200);
        });

        modelBuilder.Entity<User>(e =>
        {
            e.HasKey(u => u.Id);
            e.Property(u => u.Username).IsRequired().HasMaxLength(100);
            e.HasIndex(u => u.Username).IsUnique();
        });

        // Seed: usuario admin
        modelBuilder.Entity<User>().HasData(new User
        {
            Id = 1,
            Username = "admin",
            // SHA256 de "Admin1234!"
            PasswordHash = "XOQa2mTx6P+wrPqvpiKxQUOPOld3eF5/C4MPtz5A09Y=",
            Email = "admin@toyota.trade",
            Rol = "Admin",
            Activo = true,
            FechaCreacion = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        });
    }
}
