using JewelryApp.Models.Dtos;

namespace JewelryApp.Business.AppServices;

public interface IAccountService
{
    Task<UserTokenDto> AuthenticateAsync(LoginDto request);
    Task<UserTokenDto> RefreshAsync(UserTokenDto request);
}