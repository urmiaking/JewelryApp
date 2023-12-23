namespace JewelryApp.Shared.Requests.Products;

public record GetProductsRequest(int Page, int PageSize, string? SortDirection, string? SortLabel, string? SearchString);
