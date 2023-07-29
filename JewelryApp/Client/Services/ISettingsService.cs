using JewelryApp.Models.Dtos;

namespace JewelryApp.Client.Services;

public interface ISettingsService
{
    Task<bool> AddApiKey(ApiKeyDto apiKey);
    Task<IEnumerable<ApiKeyDto>?> GetApiKeys();
    Task<bool> SetActiveApiKey(ApiKeyDto apiKey);
    Task<bool> ChangePassowrdAsync(ChangePasswordDto passwordDto);
}
