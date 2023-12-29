using System.Net.Http.Json;
using ErrorOr;
using JewelryApp.Client.Services.Extensions;
using JewelryApp.Shared.Abstractions;
using JewelryApp.Shared.Attributes;
using JewelryApp.Shared.Common;
using JewelryApp.Shared.Requests.Authentication;
using JewelryApp.Shared.Responses.Authentication;

namespace JewelryApp.Client.Services.HttpServices;

[ScopedService<IAccountService>]
public class AccountService : IAccountService
{
    private readonly HttpClient _unauthorizedClient;
    private readonly HttpClient _authorizedClient;

    public AccountService(IHttpClientFactory httpClientFactory)
    {
        _authorizedClient = httpClientFactory.CreateClient("AuthorizedClient");
        _unauthorizedClient = httpClientFactory.CreateClient("UnauthorizedClient");
    }

    public async Task<ErrorOr<AuthenticationResponse?>> AuthenticateAsync(AuthenticationRequest request, CancellationToken token = default)
    {
        try
        {
            var response = await _unauthorizedClient.PostAsJsonAsync(Urls.Login, request, token);

            return await response.GenerateErrorOrResponseAsync<AuthenticationResponse?>(token);
        }
        catch (Exception e)
        {
            return Error.Failure(description: e.Message);
        }
    }

    public async Task<ErrorOr<AuthenticationResponse?>> RefreshAsync(RefreshTokenRequest request, CancellationToken token = default)
    {
        try
        {
            var response = await _authorizedClient.PostAsJsonAsync(Urls.RefreshToken, request, token);

            return await response.GenerateErrorOrResponseAsync<AuthenticationResponse?>(token);
        }
        catch (Exception e)
        {
            return Error.Failure(description: e.Message);
        }
    }

    public async Task<ErrorOr<ChangePasswordResponse?>> ChangePasswordAsync(ChangePasswordRequest request, CancellationToken token = default)
    {
        try
        {
            var response = await _authorizedClient.PostAsJsonAsync(Urls.ChangePassword, request, token);

            return await response.GenerateErrorOrResponseAsync<ChangePasswordResponse?>(token);
        }
        catch (Exception e)
        {
            return Error.Failure(description: e.Message);
        }
    }
}