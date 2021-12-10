using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using BlazorApp.Shared;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace ApiIsolated
{
    public class GetCoinJson
    {
        private readonly ILogger _logger;

        public GetCoinJson(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<GetCoinJson>();
        }

        // Create a single, static HttpClient
        private static HttpClient httpClient = new HttpClient();
        protected Coin[] prices;

        [Function("GetCoinJson")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

           
            prices = await httpClient.GetFromJsonAsync<Coin[]>("https://api.coingecko.com/api/v3/coins/markets?vs_currency=usd&order=market_cap_desc&per_page=6&page=1&sparkline=false&price_change_percentage=1h");
            // https://api.coingecko.com/api/v3/coins/markets?vs_currency=usd&order=market_cap_desc&per_page=10&page=1&sparkline=false&price_change_percentage=1h


            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(prices);
            return response;

        }
    }
}
