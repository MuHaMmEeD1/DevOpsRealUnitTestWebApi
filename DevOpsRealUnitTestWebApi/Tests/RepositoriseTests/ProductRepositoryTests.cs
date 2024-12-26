using DevOpsRealUnitTestWebApi.DataBase;
using DevOpsRealUnitTestWebApi.Entitys;
using DevOpsRealUnitTestWebApi.Repositorise;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace DevOpsRealUnitTestWebApi.Tests.RepositoriseTests
{
    [TestFixture]
    public class ProductRepositoryTests
    {
        private WebApplicationFactory<Program> _factory;
        private ProductRepository _repository;
        private ProductDbContext _context;

        [SetUp]
        public void SetUp()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("Default");

            _factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddDbContext<ProductDbContext>(options =>
                        options.UseSqlServer(connectionString));
                });
            });

            var scope = _factory.Services.CreateScope();
            _context = scope.ServiceProvider.GetRequiredService<ProductDbContext>();
            _repository = new ProductRepository(_context); 
        }


        [Test]
        public async Task AddProductAsync_ShouldAddProduct()
        {
            var newProduct = new Product { Name = "Test Product", Price = 100 };
            await _repository.AddProductAsync(newProduct);

            var product = await _context.Products.FirstOrDefaultAsync(p => p.Name == "Test Product");

            Assert.That(product, Is.Not.Null);
            Assert.That(product.Name, Is.EqualTo("Test Product"));
            Assert.That(product.Price, Is.EqualTo(100));
        }

        [Test]
        public async Task GetAllProductsAsync_ShouldReturnProducts()
        {
            _context.Products.RemoveRange(_context.Products);
            await _context.SaveChangesAsync();

            var product1 = new Product { Name = "Product 1", Price = 100 };
            var product2 = new Product { Name = "Product 2", Price = 200 };

            await _repository.AddProductAsync(product1);
            await _repository.AddProductAsync(product2);

            var products = await _repository.GetAllProductsAsync();

            Assert.That(products.Count(), Is.EqualTo(2));  
        }


        [Test]
        public async Task GetProductByIdAsync_ShouldReturnProduct()
        {
            var newProduct = new Product { Name = "Product by ID", Price = 150 };
            await _repository.AddProductAsync(newProduct);

            var product = await _repository.GetProductByIdAsync(newProduct.Id);

            Assert.That(product, Is.Not.Null);
            Assert.That(product.Name, Is.EqualTo("Product by ID"));
            Assert.That(product.Price, Is.EqualTo(150));
        }

        [Test]
        public async Task UpdateProductAsync_ShouldUpdateProduct()
        {
            var newProduct = new Product { Name = "Old Product", Price = 100 };
            await _repository.AddProductAsync(newProduct);

            newProduct.Name = "Updated Product";
            newProduct.Price = 200;
            await _repository.UpdateProductAsync(newProduct);

            var updatedProduct = await _repository.GetProductByIdAsync(newProduct.Id);

            Assert.That(updatedProduct.Name, Is.EqualTo("Updated Product"));
            Assert.That(updatedProduct.Price, Is.EqualTo(200));
        }

        [Test]
        public async Task DeleteProductAsync_ShouldDeleteProduct()
        {
            var newProduct = new Product { Name = "Delete Product", Price = 50 };
            await _repository.AddProductAsync(newProduct);

            await _repository.DeleteProductAsync(newProduct.Id);

            var deletedProduct = await _repository.GetProductByIdAsync(newProduct.Id);

            Assert.That(deletedProduct, Is.Null);
        }
    }
}
