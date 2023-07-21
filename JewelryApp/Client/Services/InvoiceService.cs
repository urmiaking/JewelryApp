using JewelryApp.Models.Dtos;
using System.Net.Http.Json;

namespace JewelryApp.Client.Services;

public class InvoiceService : IInvoiceService
{
    private readonly HttpClient _httpClient;

    public InvoiceService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<InvoiceTableItemDto>?> GetInvoicesAsync(int count = 0)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<InvoiceTableItemDto>>($"/api/Invoices/GetInvoices?count={count}");
        }
        catch
        {
            return new List<InvoiceTableItemDto>();
        }
    }
}