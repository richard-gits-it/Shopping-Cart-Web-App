using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using multiple_tables.models;

namespace multiple_tables.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<multiple_tables.models.Products> Products { get; set; } = default!;
        public DbSet<multiple_tables.models.Customers> Customer { get; set; } = default!;
        public DbSet<multiple_tables.models.Orders> Orders { get; set; } = default!;
        public DbSet<multiple_tables.models.OrderDetails> OrderDetails { get; set; } = default!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<OrderDetails>()
                .HasKey(od => new { od.OrderId, od.ProductId });

            modelBuilder.Entity<Orders>()
                .HasMany(o => o.OrderDetails)
                .WithOne(od => od.Order)
                .HasForeignKey(od => od.OrderId);

            modelBuilder.Entity<Categories>()
                .HasMany(c => c.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId);
        }
        public DbSet<multiple_tables.models.Categories> Categories { get; set; } = default!;
        public DbSet<multiple_tables.models.Cart> Cart { get; set; } = default!;
    }
}