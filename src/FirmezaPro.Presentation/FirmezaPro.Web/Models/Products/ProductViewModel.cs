namespace FirmezaPro.Web.Models.Products;

public class ProductViewModel
{
    public Guid Id { get; init; }
    public string Name { get; init; } = "";
    public string Description { get; init; } = "";
    public decimal Price { get; init; }
    public int Stock { get; init; }
    public bool IsActive { get; init; }
}