
namespace TestFromGitToMongo.Services.TripService
{
    public interface ITripServiceClient
    {
        List<TripDTO> Trips { get; set; }
        TripDTO TripDTO { get; set; }
        List<ChainRotationTripsDTO> ChainRotationsTripsDTO { get; set; }
        Task GetTrips();
        Task GetTripsForBike(int bikeId);
        Task<ServiceResponse<int>> AddTrip(Trip trip);
        Task GetTripAsync(string tripId);
        Task<ServiceResponse<int>> UpdateTrip(Trip trip);
        Task<ServiceResponse<bool>> DeleteTrip(string  tripId);
        Task GetChainRotationTrips(int chainId);
        Task <ServiceResponse<string>> GetChainUsedInlastTrip(int bikeId);
        Task <ServiceResponse<string>>  GetChainRotationUsedInlastTrip(int bikeId);
        Task <ServiceResponse<string>>  GetChainLastTripDate(int bikeId);

    }
}
