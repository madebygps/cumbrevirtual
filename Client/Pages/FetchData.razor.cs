using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BlazorApp.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace BlazorApp.Client
{
    public class FetchDataBase : ComponentBase
    {
        protected Coin[] prices;

        [Inject]
        public HttpClient Http {get;set;}


        protected HubConnection hubConnection;

        public bool IsConnected => hubConnection.State == HubConnectionState.Connected;
        protected override async Task OnInitializedAsync()
        {

            try{
                 prices = await Http.GetFromJsonAsync<Coin[]>("https://api.coingecko.com/api/v3/coins/markets?vs_currency=usd&order=market_cap_desc&per_page=100&page=1&sparkline=false");


            } catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
           
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