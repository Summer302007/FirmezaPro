using FirmezaPro.Domain.Entities;

namespace FirnezaPro.Domain.Interfaces
{
    public interface IProductRepository
    {
        IQueryable<Product> Query();
        Task<IReadOnlyList<Product>> GetPagedAsync(
            string? search,
            int page,
            int pageSize
        );
        Task<Product?> GetByIdAsync(Guid id);
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(Product product);
        Task<bool> ExistsAsync(Guid id);
    }
}