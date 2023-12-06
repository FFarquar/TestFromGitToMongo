using TestFromGitToMongo.Models;
using System.Net.Http.Json;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoFramework;
using TestFromGitToMongo.Data;
using System.Text.Json;

namespace TestFromGitToMongo.Services.BikeService
{
    public class BikeServiceClient : IBikeServiceClient
    {
        private readonly HttpClient _httpClient;
        const string _baseUrl = "https://graceful-deer-fedora.cyclic.app/";
        
        const string _bikesEndpoint = "bikes";

        public BikeServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public List<Bike> Bikes { get; set; } = new List<Bike>();
        public Bike Bike { get; set; } = new Bike();

        public async Task GetBike(int bikeId)
        {




            var result = await _httpClient.GetFromJsonAsync<ServiceResponse<Bike>>("api/bike/" + bikeId);
            if (result != null && result.Data != null)
                Bike = result.Data;
        }

        private void ConfigureHttpCLient()
        {
            _httpClient.BaseAddress = new Uri(_baseUrl);
        }

        public async Task GetBikes()
        {

            ConfigureHttpCLient();

            var response = await _httpClient.GetFromJsonAsync < Bike[]>(_bikesEndpoint);
            Bikes = response.ToList();

        }
    }
}
