using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace JewelryApp.Client.Services;

public class SignalRService
{
    private HubConnection? _hubConnection;
    private readonly NavigationManager _navigationManager;

    public SignalRService(NavigationManager navigationManager)
    {
        _navigationManager = navigationManager;
    }

    public async Task Connect()
    {
        var hubUrl = _navigationManager.BaseUri + "signalr-hub";
        _hubConnection = new HubConnectionBuilder()
            .WithUrl(hubUrl)
            .Build();

        await _hubConnection.StartAsync();
    }

    public void RegisterUpdateHandler(Action<string> updateHandler)
    {
        _hubConnection?.On("PriceUpdate", updateHandler);
    }
}