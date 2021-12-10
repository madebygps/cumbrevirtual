using System.Net.Http.Json;
using BlazorApp.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
namespace BlazorApp.Client
{
    public class FetchDataBase : ComponentBase
    {
        protected Coin[]? prices;

        [Inject]
        public HttpClient Http { get; set; }
        protected HubConnection hubConnection;
        public bool IsConnected => hubConnection?.State == HubConnectionState.Connected;
        protected override async Task OnInitializedAsync()
        {
            prices = await Http.GetFromJsonAsync<Coin[]>("/api/GetCoinJson");

            hubConnection = new HubConnectionBuilder().WithUrl("https://functioncryptobackend.azurewebsites.net/api").Build();

            hubConnection.On<Coin[]>("updated", (coin) =>
            {
                prices = coin;
                StateHasChanged();
            }

            );
            await hubConnection.StartAsync();
        }
    }
}