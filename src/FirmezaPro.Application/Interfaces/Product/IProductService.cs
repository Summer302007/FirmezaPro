using FirmezaPro.Application.Pagination;
using FirmezaPro.Domain.Entities;

namespace FirmezaPro.Application.Interfaces
{
    public interface IProductService
    {
        Task<PagedResult<Product>> GetPagedAsync(string? search, int page, int pageSize);
        Task<Product> GetByIdAsync(Guid id);
        Task AddProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(Guid id);
    }
}