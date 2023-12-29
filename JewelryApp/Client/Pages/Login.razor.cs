using JewelryApp.Client.Security;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using JewelryApp.Client.ViewModels;
using JewelryApp.Shared.Abstractions;
using JewelryApp.Shared.Requests.Authentication;

namespace JewelryApp.Client.Pages;

public partial class Login
{
    private readonly LoginVm _viewModel = new ();

    [Inject] private AuthenticationStateProvider AuthStateProvider { get; set; } = default!;
    [Inject] private IAccountService AccountService { get; set; } = default!;

    private async Task OnValidSubmit(EditContext context)
    {
        var request = Mapper.Map<AuthenticationRequest>(_viewModel);

        var response = await AccountService.AuthenticateAsync(request, CancellationTokenSource.Token);

        if (response.IsError)
        {
            foreach (var responseError in response.Errors)
            {
                SnackBar.Add(responseError.Description);
            }
        }
        else
        {
            SnackBar.Add("خوش آمدید!", Severity.Info);

            var userToken = response.Value;

            if (userToken != null && AuthStateProvider is AppAuthStateProvider authStateProvider)
            {
                await authStateProvider.LoginAsync(userToken.Token, userToken.RefreshToken);

                NavigationManager.NavigateTo("/");
            }
        }
    }
}

