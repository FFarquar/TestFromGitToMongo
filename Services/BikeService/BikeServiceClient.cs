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
        private readonly BikeAPIClient _bikesClient;

        public BikeServiceClient(BikeAPIClient bikesClient)
        {
            _bikesClient = bikesClient;
        }

        public List<Bike> Bikes { get; set; } = new List<Bike>();
        public Bike Bike { get; set; } = new Bike();

        public async Task GetBike(int bikeId)
        {
            Bike = await _bikesClient.GetBike(bikeId);
        }

        public async Task GetBikes()
        {
            Bikes = await _bikesClient.GetBikes();
        }
    }
}
