using DevOpsRealUnitTestWebApi.Entitys;
using Microsoft.EntityFrameworkCore;

namespace DevOpsRealUnitTestWebApi.DataBase
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Product 1", Price = 10.0 },
                new Product { Id = 2, Name = "Product 2", Price = 20.0 },
                new Product { Id = 3, Name = "Product 3", Price = 30.0 },
                new Product { Id = 4, Name = "Product 4", Price = 40.0 },
                new Product { Id = 5, Name = "Product 5", Price = 50.0 },
                new Product { Id = 6, Name = "Product 6", Price = 60.0 },
                new Product { Id = 7, Name = "Product 7", Price = 70.0 },
                new Product { Id = 8, Name = "Product 8", Price = 80.0 },
                new Product { Id = 9, Name = "Product 9", Price = 90.0 },
                new Product { Id = 10, Name = "Product 10", Price = 100.0 }
            );
        }

    }
}
