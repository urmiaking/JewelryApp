using JewelryApp.Client.Security;
using JewelryApp.Shared.Abstractions;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;

namespace JewelryApp.Client.Pages;

public partial class Logout
{
    [Inject] private AuthenticationStateProvider AuthStateProvider { get; set; } = default!;
    [Inject] private IAccountService AccountService { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        if (AuthStateProvider is AppAuthStateProvider authStateProvider)
        {
            await authStateProvider.LogoutAsync();
            NavigationManager.NavigateTo("/login");
            await AccountService.LogoutAsync(CancellationTokenSource.Token);
        }

        await base.OnInitializedAsync();
    }
}
