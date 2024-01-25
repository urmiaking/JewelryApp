namespace JewelryApp.Shared.Requests.OldGolds;

public record AddOldGoldRequest(string Name, double Weight, int InvoiceId, double Price, DateTime BuyDateTime);
