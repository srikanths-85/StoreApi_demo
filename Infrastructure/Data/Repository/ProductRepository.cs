using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly StoreContext _context;

        public ProductRepository(StoreContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<Product>> GetAllAsync()
        {
            return await _context.Products.AsNoTracking().ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
        }

        public void Update(Product product)
        {
            _context.Products.Update(product);
        }

        public void Delete(Product product)
        {
            _context.Products.Remove(product);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Products.AnyAsync(p => p.Id == id);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<string>> GetBrandsAsync()
        {
            return await _context.Products.Where(p => p.Brand != null)
                .Select(p => p.Brand)
                .Distinct()
                .ToListAsync();
        }

        public async Task<IReadOnlyList<string>> GetTypeAsync()
        {
            return await _context.Products.Where(p => p.Type != null)
                .Select(p => p.Type)
                .Distinct()
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Product>> GetProductsAsync(string? Brand, string type, string sort)
        {
            IQueryable<Product> products = _context.Products.AsQueryable();
            if (!string.IsNullOrWhiteSpace(Brand))
                products = products.Where(p => p.Brand == Brand);
            if (!string.IsNullOrWhiteSpace(type))
                products = products.Where(p => p.Type == type);

            products = sort switch
            {
                "priceAsc" => products.OrderBy(x => x.Price),
                "priceDesc" => products.OrderByDescending(x => x.Price),
                _ => products.OrderBy(x => x.Name)
            };
            return await products.ToListAsync();
        }
    }
}
