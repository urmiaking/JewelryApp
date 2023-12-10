namespace JewelryApp.Shared.Requests.Customer;

public record UpdateCustomerRequest (int InvoiceId, string Name, string PhoneNumber);