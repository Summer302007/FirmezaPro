using FirmezaPro.Application.Interfaces;
using FirmezaPro.Domain.Entities;
using FirmezaPro.Domain.Interfaces;
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

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
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
            // Aquí podés agregar lógica adicional antes de guardar
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