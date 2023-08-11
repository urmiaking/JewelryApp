using JewelryApp.Client.Security;
using JewelryApp.Models.Dtos;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Net.Http.Json;
using System.Text.Json;

namespace JewelryApp.Client.Pages;

public partial class Login
{
    private readonly LoginDto _model = new ();

    [Inject] private AuthenticationStateProvider AuthStateProvider { get; set; } = default!;

    private async Task OnValidSubmit(EditContext context)
    {
        var httpResponse = await UnauthorizedHttpClient.PostAsJsonAsync("login", _model);

        if (httpResponse.IsSuccessStatusCode)
        {
            SnackBar.Add("خوش آمدید!", Severity.Info);

            var responseContent = await httpResponse.Content.ReadAsStringAsync();
            var userToken = JsonSerializer.Deserialize<UserTokenDto>(responseContent, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            if (userToken != null && AuthStateProvider is AppAuthStateProvider authStateProvider)
            {
                await authStateProvider.LoginAsync(userToken.Token, userToken.RefreshToken);

                NavigationManager.NavigateTo("/");
            }
        }
        else
        {
            SnackBar.Add("نام کاربری یا رمز عبور اشتباه است!", Severity.Error);
        }
    }
}

