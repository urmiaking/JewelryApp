using JewelryApp.Models.Dtos;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Net.Http.Json;

namespace JewelryApp.Client.Pages;

public partial class Settings
{
    MudListItem selectedItem;
    object selectedValue = 1;
    Color _color = Color.Success;

    [Parameter]
    public ChangePasswordDto PasswordModel { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
    }

    async Task OnPasswordChangeSubmit()
    {
        await PutAsync("/changepassword", PasswordModel);
    }
}

