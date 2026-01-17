using FirmezaPro.Application.Interfaces;
using FirmezaPro.Domain.Entities;
using FirmezaPro.Web.Models.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FirmezaPro.Web.Controllers
{
    [Authorize] // Todos deben estar logueados
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        // 👉 Método único para mostrar productos según rol
        public async Task<IActionResult> Index(string? search, int page = 1)
        {
            const int pageSize = 10;

            var result = await _productService.GetPagedAsync(search, page, pageSize);

            var productsVm = result.Items.Select(p => new ProductViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Stock = p.Stock,
                IsActive = p.IsActive
            }).ToList();

            if (User.IsInRole("Admin"))
            {
                var adminVm = new AdminProductListViewModel
                {
                    Products = productsVm,
                    Page = page,
                    PageSize = pageSize,
                    HasNextPage = result.HasNextPage,
                    Search = search
                };

                return View("AdminIndex", adminVm);
            }

            var userVm = new ProductListViewModel
            {
                Products = productsVm,
                Page = page,
                HasNextPage = result.HasNextPage,
                Search = search
            };

            return View("Index", userVm);
        }
        
        // 👑 ADMIN → Crear producto
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View(new CreateProductViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateProductViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var product = new Product(
                model.Name,
                model.Description,
                model.Price,
                model.Stock,
                model.IsActive
            );
            await _productService.AddProductAsync(product);
            return RedirectToAction(nameof(Index));
        }

        // 👑 ADMIN → Editar producto
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null)
                return NotFound();

            return View(product); // Views/Product/Edit.cshtml
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(Guid id, Product model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                await _productService.UpdateProductAsync(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        // 👑 ADMIN → Eliminar producto
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null)
                return NotFound();

            return View(product); // Views/Product/Delete.cshtml
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            try
            {
                await _productService.DeleteProductAsync(id);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                var product = await _productService.GetByIdAsync(id);
                return View(product);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}

