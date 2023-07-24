using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JewelryApp.Client.Security;

public class AppAuthStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorage;
    private AuthenticationState Anonymous => new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

    public AppAuthStateProvider(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");

            if (string.IsNullOrWhiteSpace(token))
                return Anonymous;

            var expireDate = JwtParser.GetExpireDate(token);
            if (expireDate < DateTime.UtcNow)
                return Anonymous;

            var claims = JwtParser.ParseClaimsFromJwt(token);

            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(claims, "jwtAuthType")));
        }
        catch (Exception)
        {
            return Anonymous;
        }
    }

    public async Task LoginAsync(string token, Guid refreshToken)
    {
        await _localStorage.SetItemAsync("authToken", token);
        await _localStorage.SetItemAsync("refreshToken", refreshToken);

        NotifyStateChange();
    }

    public async Task LogoutAsync()
    {
        await _localStorage.RemoveItemAsync("authToken");
        await _localStorage.RemoveItemAsync("refreshToken");

        NotifyStateChange();
    }

    public void NotifyStateChange()
    {
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}
