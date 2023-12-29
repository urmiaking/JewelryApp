using System.Security.Claims;
using Microsoft.AspNetCore.Components;
using JewelryApp.Client.ViewModels;
using JewelryApp.Shared.Abstractions;
using JewelryApp.Shared.Requests.Authentication;
using Microsoft.AspNetCore.Components.Authorization;

namespace JewelryApp.Client.Pages;

public partial class Settings
{
    [Parameter]
    public ChangePasswordVm PasswordModel { get; set; } = new();

    [Inject] private IAccountService AccountService { get; set; } = default!;
    [Inject] private AuthenticationStateProvider AuthStateProvider { get; set; } = default!;


    async Task OnPasswordChangeSubmit()
    {
        PasswordModel.UserName = (await AuthStateProvider.GetAuthenticationStateAsync()).User.Claims.FirstOrDefault(x =>
            x.Type == ClaimTypes.Name)?.Value;

        var request = Mapper.Map<ChangePasswordRequest>(PasswordModel);
        
        await AccountService.ChangePasswordAsync(request, CancellationTokenSource.Token);
    }

    private void Close()
    {
        
    }
}

