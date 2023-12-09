namespace JewelryApp.Shared.Requests.Invoices;

public record GetInvoiceTableRequest(int Page, int PageSize, string SortDirection, string SortLabel, string SearchString);
