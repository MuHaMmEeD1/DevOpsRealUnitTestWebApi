using DevOpsRealUnitTestWebApi.Entitys;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;
using System.Net.Http.Json;
using System.Linq;
using System.Collections.Generic;

namespace DevOpsRealUnitTestWebApi.Tests.ControllerTests
{
    [TestFixture]
    public class ProductTests
    {
        private WebApplicationFactory<Program> _factory;
        private HttpClient _client;

        [SetUp]
        public void SetUp()
        {
            _factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services => { });
            });

            _client = _factory.CreateClient();
        }

        [Test]
        public async Task GetProducts_ReturnsOkResponse()
        {
            var response = await _client.GetAsync("/api/product?top=10");
            Assert.That(response.IsSuccessStatusCode, Is.True);

            var products = await response.Content.ReadFromJsonAsync<List<Product>>();
            Assert.That(products, Is.Not.Null);
            Assert.That(products.Count, Is.GreaterThan(0));
        }

        [Test]
        public async Task GetProduct_ReturnsCorrectProduct()
        {
            var response = await _client.GetAsync("/api/product?top=10");
            Assert.That(response.IsSuccessStatusCode, Is.True);

            var products = await response.Content.ReadFromJsonAsync<List<Product>>();
            var product = products?.FirstOrDefault();
            Assert.That(product, Is.Not.Null);

            if (product != null)
            {
                var responseProduct = await _client.GetAsync($"/api/product/{product.Id}");
                Assert.That(responseProduct.IsSuccessStatusCode, Is.True);

                var retrievedProduct = await responseProduct.Content.ReadFromJsonAsync<Product>();
                Assert.That(retrievedProduct, Is.Not.Null);
                Assert.That(retrievedProduct.Id, Is.EqualTo(product.Id));
            }
        }

        [Test]
        public async Task PostProduct_AddsNewProduct()
        {
            var newProduct = new Product
            {
                Name = "New Product",
                Price = 500
            };

            var response = await _client.PostAsJsonAsync("/api/product", newProduct);
            Assert.That(response.IsSuccessStatusCode, Is.True);

            var createdProduct = await response.Content.ReadFromJsonAsync<Product>();
            Assert.That(createdProduct, Is.Not.Null);
            Assert.That(createdProduct?.Name, Is.EqualTo(newProduct.Name));
            Assert.That(createdProduct?.Price, Is.EqualTo(newProduct.Price));
        }

        [Test]
        public async Task PutProduct_UpdatesProduct()
        {
            var response = await _client.GetAsync("/api/product?top=10");
            Assert.That(response.IsSuccessStatusCode, Is.True);

            var products = await response.Content.ReadFromJsonAsync<List<Product>>();
            var product = products?.FirstOrDefault();
            Assert.That(product, Is.Not.Null);

            if (product != null)
            {
                product.Name = "Updated Product";
                product.Price = 600;

                var updateResponse = await _client.PutAsJsonAsync($"/api/product/{product.Id}", product);
                Assert.That(updateResponse.IsSuccessStatusCode, Is.True);

                var updatedProduct = await updateResponse.Content.ReadFromJsonAsync<Product>();
                Assert.That(updatedProduct, Is.Not.Null);
                Assert.That(updatedProduct?.Name, Is.EqualTo("Updated Product"));
                Assert.That(updatedProduct?.Price, Is.EqualTo(600));
            }
        }


        [Test]
        public async Task DeleteProduct_RemovesProduct()
        {
            var response = await _client.GetAsync("/api/product?top=10");
            Assert.That(response.IsSuccessStatusCode, Is.True);

            var products = await response.Content.ReadFromJsonAsync<List<Product>>();
            var product = products?.FirstOrDefault();
            Assert.That(product, Is.Not.Null);

            if (product != null)
            {
                var deleteResponse = await _client.DeleteAsync($"/api/product/{product.Id}");
                Assert.That(deleteResponse.IsSuccessStatusCode, Is.True);

                var getDeletedProductResponse = await _client.GetAsync($"/api/product/{product.Id}");
                Assert.That(getDeletedProductResponse.IsSuccessStatusCode, Is.False);
            }
        }
    }
}
