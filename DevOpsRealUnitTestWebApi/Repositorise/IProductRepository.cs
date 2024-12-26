using DevOpsRealUnitTestWebApi.Entitys;

namespace DevOpsRealUnitTestWebApi.Repositorise
{
    public interface IProductRepository
    {
        Task AddProductAsync(Product product);
        Task<Product> GetProductByIdAsync(int id);
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(int id);
    }
}
