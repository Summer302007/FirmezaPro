using FirmezaPro.Domain.Entities;
using FirmezaPro.Infrastructure.Persistence;
using FirnezaPro.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FirmezaPro.Infrastructure.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public IQueryable<Product> Query()
        {
            return _context.Products.AsNoTracking();
        }
        
        public async Task<IReadOnlyList<Product>> GetPagedAsync(
            string? search,
            int page,
            int pageSize)
        {
            var query = _context.Products.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(p =>
                    p.Name.Contains(search) ||
                    p.Description.Contains(search));
            }

            return await query
                .OrderBy(p => p.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize + 1)
                .ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(Guid id)
        {
            return await _context.Products.FindAsync(id);
        }


        public async Task AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Product product)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Products.AnyAsync(p => p.Id == id);
        }
    }
}