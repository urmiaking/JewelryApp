using JewelryApp.Models.Dtos.Authentication;

namespace JewelryApp.Business.AppServices;

public interface IAccountService
{
    Task<UserTokenDto> AuthenticateAsync(LoginDto request);
    Task<UserTokenDto> RefreshAsync(UserTokenDto request);
}