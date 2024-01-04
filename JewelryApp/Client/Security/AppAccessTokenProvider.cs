using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Net.Http.Json;
using System.Text.Json;
using JewelryApp.Shared.Attributes;
using JewelryApp.Shared.Responses.Authentication;
using JewelryApp.Shared.Requests.Authentication;
using JewelryApp.Shared.Common;

namespace JewelryApp.Client.Security;

[ScopedService<IAccessTokenProvider>]
internal class AppAccessTokenProvider : IAccessTokenProvider
{
    private readonly ILocalStorageService _localStorage;
    private readonly HttpClient _unauthorizedClient;

    public AppAccessTokenProvider(ILocalStorageService localStorage, IHttpClientFactory clientFactory)
    {
        _localStorage = localStorage;
        _unauthorizedClient = clientFactory.CreateClient("UnauthorizedClient");
    }

    public async ValueTask<AccessTokenResult> RequestAccessToken()
    {
        var token = await GetTokenAsync();
        var result = AccessTokenResultStatus.Success;

        if (string.IsNullOrEmpty(token.Value) || token.Expires <= DateTime.UtcNow)
            result = AccessTokenResultStatus.RequiresRedirect;

        InteractiveRequestOptions requestOptions = new()
        {
            Interaction = InteractionType.SignIn,
            ReturnUrl = "/login",
        };
        return new AccessTokenResult(result, token, "/login", requestOptions);
    }

    public async ValueTask<AccessTokenResult> RequestAccessToken(AccessTokenRequestOptions options)
    {
        var token = await GetTokenAsync();
        var result = AccessTokenResultStatus.Success;

        if (string.IsNullOrEmpty(token.Value) || token.Expires <= DateTime.UtcNow)
            result = AccessTokenResultStatus.RequiresRedirect;

        InteractiveRequestOptions requestOptions = new()
        {
            Interaction = InteractionType.SignIn,
            ReturnUrl = "/login",
        };

        return new AccessTokenResult(result, token, options.ReturnUrl, requestOptions);
    }

    private async Task<AccessToken> GetTokenAsync()
    {
        if (!await _localStorage.ContainKeyAsync("authToken"))
            return GetAnonymousToken();

        var token = await _localStorage.GetItemAsync<string>("authToken");
        var expire = JwtParser.GetExpireDate(token);

        if (expire <= DateTime.UtcNow)
        {
            var refreshToken = await _localStorage.GetItemAsync<Guid>("refreshToken");
            var refreshResult = await RefreshToken(token, refreshToken);

            if (refreshResult is null)
                return GetAnonymousToken();

            await _localStorage.SetItemAsync("authToken", refreshResult.Token);
            await _localStorage.SetItemAsync("refreshToken", refreshResult.RefreshToken);

            token = refreshResult.Token;

            if (!string.IsNullOrEmpty(token))
                expire = JwtParser.GetExpireDate(token);
        }

        return new AccessToken { Value = token, Expires = expire };
    }

    private async Task<AuthenticationResponse?> RefreshToken(string token, Guid refreshToken)
    {
        var model = new RefreshTokenRequest(token, refreshToken);

        var responseMessage = await _unauthorizedClient.PostAsJsonAsync(Urls.RefreshToken, model);

        if (responseMessage.IsSuccessStatusCode)
        {
            var content = await responseMessage.Content.ReadAsStringAsync();
            var response = JsonSerializer.Deserialize<AuthenticationResponse>(content, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            return response;
        }

        return null;
    }

    private static AccessToken GetAnonymousToken()
    {
        return new AccessToken { Expires = DateTime.Now };
    }
}
