using System.Net.Http.Json;

using TestFromGitToMongo.Models;

namespace TestFromGitToMongo.Services.BikeService
{
    public interface IBikeServiceClient
    {
        List<Bike> Bikes { get; set; }
        Bike Bike { get; set; }
        Task GetBikes();
        Task GetBike(int bikeId);
    }
}
