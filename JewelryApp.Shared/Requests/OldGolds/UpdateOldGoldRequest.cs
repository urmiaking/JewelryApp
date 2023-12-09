namespace JewelryApp.Shared.Requests.OldGolds;

public record UpdateOldGoldRequest(int Id, string Name, double Weight, int InvoiceId, double Price);