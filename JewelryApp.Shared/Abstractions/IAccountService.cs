using ErrorOr;
using JewelryApp.Shared.Requests.Authentication;
using JewelryApp.Shared.Responses.Authentication;

namespace JewelryApp.Shared.Abstractions;

public interface IAccountService
{
    Task<ErrorOr<AuthenticationResponse?>> AuthenticateAsync(AuthenticationRequest request, CancellationToken token = default);
    Task<ErrorOr<AuthenticationResponse?>> RefreshAsync(RefreshTokenRequest request, CancellationToken token = default);
    Task<ErrorOr<ChangePasswordResponse?>> ChangePasswordAsync(ChangePasswordRequest request, CancellationToken token = default);
    Task LogoutAsync(CancellationToken token = default);
}