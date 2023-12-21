﻿using System.Text;
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
            _client.BaseAddress = new Uri(Settings.API_BaseUrl);
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
            using (var response = await _client.GetAsync("bikes/bike/"+ bikeId, HttpCompletionOption.ResponseHeadersRead))
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
            using (var response = await _client.GetAsync("trips/getalltrips", HttpCompletionOption.ResponseHeadersRead))
            {
                response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();

                var Trips = await JsonSerializer.DeserializeAsync<List<TripDTO>>(stream, _options);
                return Trips;

            }
        }

        public async Task<List<TripDTO>> GetTripsForBike(int bikeId)
        {
            using (var response = await _client.GetAsync("trips/gettripsforbike/"+bikeId, HttpCompletionOption.ResponseHeadersRead))
            {
                response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();

                var Trips = await JsonSerializer.DeserializeAsync<List<TripDTO>>(stream, _options);
                return Trips;

            }
        }

        public async Task<Trip> AddTrip(Trip trip)
        {
            var jsonString = JsonSerializer.Serialize(trip);
            Console.WriteLine(jsonString);
            //using (var response = await _client.PostAsJsonAsync("trips/addtrip", jsonString))
            using (var response = await _client.PostAsync("trips/addtrip", new StringContent(jsonString, Encoding.UTF8, "application/json")))
            {
                response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();

                var tripRes = await JsonSerializer.DeserializeAsync<Trip>(stream, _options);
                return tripRes;

            }


        }
        public async Task<List<ChainSummaryDTO>> GetChains(int bikeId)
        {
            using (var response = await _client.GetAsync("chains/" + bikeId, HttpCompletionOption.ResponseHeadersRead))
            {
                response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();

                var Chains = await JsonSerializer.DeserializeAsync<List<ChainSummaryDTO>>(stream, _options);
                return Chains;

            }
        }
    }

}
