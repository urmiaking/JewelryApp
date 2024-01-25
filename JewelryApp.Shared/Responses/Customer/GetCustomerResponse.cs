namespace JewelryApp.Shared.Responses.Customer;

public record GetCustomerResponse(int Id, string Name, string? PhoneNumber, string NationalCode);
