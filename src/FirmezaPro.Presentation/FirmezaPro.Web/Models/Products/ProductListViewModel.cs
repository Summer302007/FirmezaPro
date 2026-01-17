namespace FirmezaPro.Web.Models.Products;

public class ProductListViewModel
{
    public IReadOnlyList<ProductViewModel> Products { get; init; } = [];
    public int Page { get; init; }
    public bool HasNextPage { get; init; }
    public string? Search { get; init; }
}