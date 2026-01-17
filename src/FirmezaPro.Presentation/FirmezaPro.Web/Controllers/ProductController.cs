using FirmezaPro.Application.Interfaces;
using FirmezaPro.Domain.Entities;
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
        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetAllAsync();

            // Detectar rol
            if (User.IsInRole("Admin"))
                return View("AdminIndex", products); // Vista del Admin
            else
                return View("Index", products);      // Vista del Customer
        }

        // 👑 ADMIN → Crear producto
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View(); // Views/Product/Create.cshtml
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(Product model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _productService.AddProductAsync(model);
            return RedirectToAction(nameof(Index)); // Redirige al método Index que detecta rol
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

