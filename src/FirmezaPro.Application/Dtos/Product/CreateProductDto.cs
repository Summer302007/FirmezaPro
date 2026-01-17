using System.ComponentModel.DataAnnotations;

namespace FirmezaPro.Application.Dtos.Product;

public class CreateProductDto
{
    [Required(ErrorMessage = "The Name is required.")]
    [StringLength(100, MinimumLength = 3)]
    [RegularExpression(@"^[a-zA-Z0-9\s\-]+$",
        ErrorMessage = "The Name can only contain letters, numbers, spaces, and hyphens.")]
    public required string Name { get; set; }

    [Required(ErrorMessage = "The Description is required.")]
    [StringLength(500)]
    public required string Description { get; set; }

    [Required]
    [Range(0.01, 1_000_000)]
    public decimal Price { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public int Stock { get; set; }
}