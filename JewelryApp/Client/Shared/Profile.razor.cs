using System.Security.Claims;
using System.Timers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Timer = System.Timers.Timer;

namespace JewelryApp.Client.Shared;

public partial class Profile
{
    private Timer? _timer;
    private string? _currentDateTime;
    private string? _username;

    [CascadingParameter]
    private Task<AuthenticationState> AuthenticationStateTask { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        _timer = new Timer(1000);
        _timer.Elapsed += TimerElapsed;
        _timer.Start();

        _currentDateTime = DateTime.Now.ToShortTimeString();

        var authenticationState = await AuthenticationStateTask;
        var user = authenticationState.User;

        _username = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
        await base.OnInitializedAsync();
    }

    private void TimerElapsed(object? sender, ElapsedEventArgs e)
    {
        _currentDateTime = DateTime.Now.ToShortTimeString();
        InvokeAsync(StateHasChanged);
    }
}
