using System.ComponentModel.DataAnnotations;

namespace FirmezaPro.Application.Dtos.Product;

public class CreateProductDto
{
    [Required(ErrorMessage = "The Name is required.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "The Name must be between 3 and 100 characters.")]
    // Regex: Permite letras (a-z), números, espacios y guiones (-). Evita símbolos como @, $, %, <, >
    [RegularExpression(@"^[a-zA-Z0-9\s\-]+$", ErrorMessage = "The Name can only contain letters, numbers, spaces, and hyphens.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "The Description is required.")]
    [StringLength(500, ErrorMessage = "The Description cannot exceed 500 characters.")]
    public string Description { get; set; }

    [Required(ErrorMessage = "The Price is required.")]
    // Rango: Evita números negativos y pone un tope lógico (ej. 1 millón)
    [Range(0.01, 1000000, ErrorMessage = "The Price must be a positive value greater than 0.")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "The Stock Quantity is required.")]
    // Rango: Permite 0 (sin stock) pero no negativos
    [Range(0, int.MaxValue, ErrorMessage = "The Stock Quantity cannot be negative.")]
    public int StockQuantity { get; set; }
}