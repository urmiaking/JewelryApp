namespace JewelryApp.Shared.Responses.InvoiceItems;

public record RemoveInvoiceItemResponse(int Id, int InvoiceId, int ProductId);