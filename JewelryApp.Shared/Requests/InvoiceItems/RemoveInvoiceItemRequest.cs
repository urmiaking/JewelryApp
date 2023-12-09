namespace JewelryApp.Shared.Requests.InvoiceItems;

public record RemoveInvoiceItemRequest(int InvoiceId, int ProductId);