using DevOpsRealUnitTestWebApi.DataBase;
using DevOpsRealUnitTestWebApi.Entitys;
using DevOpsRealUnitTestWebApi.Repositorise;
using DevOpsRealUnitTestWebApi.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevOpsRealUnitTestWebApi.Tests.ServicesTests
{
    [TestFixture]
    public class ProductServiceTests
    {
        private ProductService _productService;
        private ProductDbContext _context;
        private WebApplicationFactory<Program> _factory;
        private ProductRepository _repository;



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
            _productService = new ProductService(_repository);
        }
        [Test]
        public async Task AddProductAsync_ShouldAddProduct()
        {
            var newProduct = new Product { Name = "Test Product", Price = 100 };

            await _productService.AddProductAsync(newProduct);

            var product = await _context.Products.FirstOrDefaultAsync(p => p.Name == "Test Product");
            Assert.That(product, Is.Not.Null);
            Assert.That(product.Name, Is.EqualTo("Test Product"));
            Assert.That(product.Price, Is.EqualTo(100));
        }

        [Test]
        public async Task GetProductByIdAsync_ShouldReturnProduct_WhenProductExists()
        {
            var newProduct = new Product { Name = "Product by ID", Price = 150 };
            await _productService.AddProductAsync(newProduct);

            var result = await _productService.GetProductByIdAsync(newProduct.Id);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("Product by ID"));
            Assert.That(result.Price, Is.EqualTo(150));
        }

        [Test]
        public async Task GetAllProductsAsync_ShouldReturnListOfProducts()
        {
            _context.Products.RemoveRange(_context.Products);
            var product1 = new Product { Name = "Product 1", Price = 100 };
            var product2 = new Product { Name = "Product 2", Price = 200 };
            await _productService.AddProductAsync(product1);
            await _productService.AddProductAsync(product2);

            var result = await _productService.GetAllProductsAsync();

            Assert.That(result.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task UpdateProductAsync_ShouldUpdateProduct()
        {
            var productToUpdate = new Product { Name = "Old Product", Price = 100 };
            await _productService.AddProductAsync(productToUpdate);

            productToUpdate.Name = "Updated Product";
            productToUpdate.Price = 200;
            await _productService.UpdateProductAsync(productToUpdate);

            var updatedProduct = await _productService.GetProductByIdAsync(productToUpdate.Id);
            Assert.That(updatedProduct.Name, Is.EqualTo("Updated Product"));
            Assert.That(updatedProduct.Price, Is.EqualTo(200));
        }

        [Test]
        public async Task DeleteProductAsync_ShouldDeleteProduct()
        {
           
            var productToDelete = new Product { Name = "Product to Delete", Price = 100 };
            await _productService.AddProductAsync(productToDelete);

            await _productService.DeleteProductAsync(productToDelete.Id);

            var deletedProduct = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == productToDelete.Id);
            Assert.That(deletedProduct, Is.Null);
        }
    }
}
