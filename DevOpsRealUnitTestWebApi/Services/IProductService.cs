using DevOpsRealUnitTestWebApi.Entitys;

namespace DevOpsRealUnitTestWebApi.Services
{
    public interface IProductService
    {
        Task AddProductAsync(Product product);
        Task<Product> GetProductByIdAsync(int id);
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(int id);
    }
}
