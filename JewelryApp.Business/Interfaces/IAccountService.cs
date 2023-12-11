using ErrorOr;
using JewelryApp.Shared.Requests.Authentication;
using JewelryApp.Shared.Responses.Authentication;

namespace JewelryApp.Application.Interfaces;

public interface IAccountService
{
    Task<ErrorOr<AuthenticationResponse?>> AuthenticateAsync(AuthenticationRequest request);
    Task<ErrorOr<AuthenticationResponse?>> RefreshAsync(RefreshTokenRequest request);
    Task<ErrorOr<ChangePasswordResponse?>> ChangePasswordAsync(ChangePasswordRequest request);
}