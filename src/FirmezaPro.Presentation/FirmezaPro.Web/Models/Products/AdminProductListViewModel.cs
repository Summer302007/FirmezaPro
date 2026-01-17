namespace FirmezaPro.Web.Models.Products;

public class AdminProductListViewModel
{
    public IReadOnlyList<ProductViewModel> Products { get; init; } = [];
    public int Page { get; init; }
    public int PageSize { get; init; }
    public bool HasNextPage { get; init; }
    public string? Search { get; init; }
}