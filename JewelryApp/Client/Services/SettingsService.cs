using JewelryApp.Data.Models;
using JewelryApp.Models.Dtos;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text;

namespace JewelryApp.Client.Services;

public class SettingsService : ISettingsService
{
    private readonly HttpClient _httpClient;

    public SettingsService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> AddApiKey(ApiKeyDto apiKey)
    {
        try
        {
            var content = new StringContent(JsonConvert.SerializeObject(apiKey), Encoding.UTF8, "application/json");

            var result = await _httpClient.PostAsync($"/updateapikey", content);

            return result.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> ChangePassowrdAsync(ChangePasswordDto passwordDto)
    {
        try
        {
            var content = new StringContent(JsonConvert.SerializeObject(passwordDto), Encoding.UTF8, "application/json");

            var result = await _httpClient.PostAsync($"/changepassword", content);

            return result.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<IEnumerable<ApiKeyDto>?> GetApiKeys()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<ApiKeyDto>>($"/apikeys");
        }
        catch
        {
            return new List<ApiKeyDto>();
        }
    }

    public async Task<bool> SetActiveApiKey(ApiKeyDto apiKey)
    {
        try
        {
            var content = new StringContent(JsonConvert.SerializeObject(apiKey), Encoding.UTF8, "application/json");

            var result = await _httpClient.PostAsync($"/setactiveapikey", content);

            return result.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}
