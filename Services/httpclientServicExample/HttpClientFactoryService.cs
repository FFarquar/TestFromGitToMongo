using System.Text.Json;
using TestFromGitToMongo.Clients;
using TestFromGitToMongo.Models;

namespace TestFromGitToMongo.Services.httpclientServicExample
{
    public class HttpClientFactoryService : IHttpClientServiceImplementation
    {
        private readonly IBrowserStorageService _browserStorageService;
        private readonly BikeAPIClient _bikesClient;
        private JsonSerializerOptions _options;
        private readonly IHttpClientFactory _httpClientFactory;

        public List<Bike> Bikes { get; set; } = new List<Bike>();

        public HttpClientFactoryService(IHttpClientFactory httpClientFactory, IBrowserStorageService browserStorageService, BikeAPIClient bikesClient)
        {
            _httpClientFactory = httpClientFactory;
            _browserStorageService = browserStorageService;
            _bikesClient = bikesClient;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task Execute()
        {
            //await GetBikesWithHttpClientFactory();
            await GetBikesWithTypedClient();
        }

        public Task GetBikes()
        {
            throw new NotImplementedException();
        }

        public Task GetBikesWithStream()
        {
            throw new NotImplementedException();
        }

        private async Task GetBikesWithHttpClientFactory()
        {
            var httpClient = _httpClientFactory.CreateClient("BikesAPI");
            using (var response = await httpClient.GetAsync("bikes", HttpCompletionOption.ResponseHeadersRead))
            {
                response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();
                Bikes = await JsonSerializer.DeserializeAsync<List<Bike>>(stream, _options);
            }
        }

        //Example of using a named client
        //private async Task GetBikesWithTypedClient()
        //{
        //    using (var response = await _bikesClient._client.GetAsync("bikes", HttpCompletionOption.ResponseHeadersRead))
        //    {
        //        response.EnsureSuccessStatusCode();
        //        var stream = await response.Content.ReadAsStreamAsync();
        //        Bikes = await JsonSerializer.DeserializeAsync<List<Bike>>(stream, _options);
        //    }
        //}


        //Example of a typed client where all processing is offloaded to that client
        private async Task GetBikesWithTypedClient()
        {
            Bikes = await _bikesClient.GetBikes();
        }
        
    }
}
