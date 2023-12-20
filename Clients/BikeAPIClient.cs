using System.Text.Json;
using TestFromGitToMongo.Models;

namespace TestFromGitToMongo.Clients
{
    //Example of a typed client
    public class BikeAPIClient
    {
        public HttpClient _client { get; }
        private readonly JsonSerializerOptions _options;
        private readonly IBrowserStorageService _browserStorageService;

        public BikeAPIClient(HttpClient client, IBrowserStorageService browserStorageService )
        {
            _client = client;
            _browserStorageService = browserStorageService;
            _client.BaseAddress = new Uri("http://localhost:3000/");
            _client.Timeout = new TimeSpan(0, 0, 30);
            _client.DefaultRequestHeaders.Clear();

            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<List<Bike>> GetBikes()
        {
            using (var response = await _client.GetAsync("bikes", HttpCompletionOption.ResponseHeadersRead))
            {
                response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();
                var Bikes  = await JsonSerializer.DeserializeAsync<List<Bike>>(stream, _options);
                return Bikes;
            }
        }

        public async Task<Bike> GetBike(int bikeId)
        {
            using (var response = await _client.GetAsync("bike/"+ bikeId, HttpCompletionOption.ResponseHeadersRead))
            {
                response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();

                stream.Position = 0;        //Deserialize doesent work without this

                var bike = await JsonSerializer.DeserializeAsync<Bike>(stream, _options);
                return bike;

            }
        }

        public async Task<List<TripDTO>> GetTrips()
        {
            using (var response = await _client.GetAsync("trips", HttpCompletionOption.ResponseHeadersRead))
            {
                response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();

                var Trips = await JsonSerializer.DeserializeAsync<List<TripDTO>>(stream, _options);
                return Trips;

            }
        }

        public async Task<List<TripDTO>> GetTripsForBike(int bikeId)
        {
            using (var response = await _client.GetAsync("trips/"+bikeId, HttpCompletionOption.ResponseHeadersRead))
            {
                response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();

                var Trips = await JsonSerializer.DeserializeAsync<List<TripDTO>>(stream, _options);
                return Trips;

            }
        }
    }

}
