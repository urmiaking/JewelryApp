namespace JewelryApp.Shared.Requests.Invoices;

public record GetInvoiceListRequest(int Page, int PageSize, string SortDirection, string? SortLabel, string? SearchString);
