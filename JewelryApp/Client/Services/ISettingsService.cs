using JewelryApp.Models.Dtos;

namespace JewelryApp.Client.Services;

public interface ISettingsService
{
    Task<bool> ChangePasswordAsync(ChangePasswordDto passwordDto);
}
