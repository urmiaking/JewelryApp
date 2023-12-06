using JewelryApp.Models.Dtos.AuthenticationDtos;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Net.Http.Json;

namespace JewelryApp.Client.Pages;

public partial class Settings
{
    [Parameter]
    public ChangePasswordDto PasswordModel { get; set; } = new();

    async Task OnPasswordChangeSubmit()
    {
        await PutAsync("/changepassword", PasswordModel);
    }

    private void Close()
    {
        
    }
}

