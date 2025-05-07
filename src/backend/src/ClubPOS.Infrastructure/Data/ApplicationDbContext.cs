using ClubPOS.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace ClubPOS.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<CashBalance> CashBalance { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Sale>()
                .Property(s => s.TotalPrice)
                .HasPrecision(10, 2);

            modelBuilder.Entity<CashBalance>()
                .Property(c => c.Balance)
                .HasPrecision(10, 2);
        }
    }
} 