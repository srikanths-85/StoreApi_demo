using Core.Entities;

namespace Core.Interfaces
{
    public interface IProductRepository
    {
        Task<IReadOnlyList<Product>> GetProductsAsync(string Brand, string type, string sort);
        Task<IReadOnlyList<Product>> GetAllAsync();
        Task<Product> GetByIdAsync(int id);
        Task<IReadOnlyList<string>> GetBrandsAsync();
        Task<IReadOnlyList<string>> GetTypeAsync();
        Task AddAsync(Product product);
        void Update(Product product);
        void Delete(Product product);
        Task<bool> ExistsAsync(int id);
        Task<int> SaveChangesAsync();
    }
}
