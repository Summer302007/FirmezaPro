using FirmezaPro.Application.Interfaces;
using FirmezaPro.Application.Pagination;
using FirmezaPro.Domain.Entities;
using FirnezaPro.Domain.Interfaces;

namespace FirmezaPro.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<PagedResult<Product>> GetPagedAsync(string? search,
            int page,
            int pageSize)
        {
            var items = await _repository.GetPagedAsync(search, page, pageSize);

            return new PagedResult<Product>
            {
                Items = items.Take(pageSize).ToList(),
                Page = page,
                PageSize = pageSize,
                HasNextPage = items.Count > pageSize
            };
        }
        
        public async Task<Product> GetByIdAsync(Guid id)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null)
                throw new Exception("Producto no encontrado");

            return product;
        }

        public async Task AddProductAsync(Product product)
        {
            await _repository.AddAsync(product);
        }

        public async Task UpdateProductAsync(Product product)
        {
            if (!await _repository.ExistsAsync(product.Id))
                throw new Exception("Producto no encontrado");

            await _repository.UpdateAsync(product);
        }

        public async Task DeleteProductAsync(Guid id)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null)
                throw new Exception("Producto no encontrado");

            await _repository.DeleteAsync(product);
        }
    }
}