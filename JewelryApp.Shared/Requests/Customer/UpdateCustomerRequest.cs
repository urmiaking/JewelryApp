namespace JewelryApp.Shared.Requests.Customer;

public record UpdateCustomerRequest (int Id, string Name, string PhoneNumber);