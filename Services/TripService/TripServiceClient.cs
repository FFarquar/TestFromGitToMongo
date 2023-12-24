
using System.Net.Http.Json;


namespace TestFromGitToMongo.Services.TripService
{
    public class TripServiceClient : ITripServiceClient
    {
//        private readonly HttpClient _http;
        private readonly BikeAPIClient _apiClient;

        public TripServiceClient(BikeAPIClient bikesClient)
        {
  //          _http = http;
            _apiClient = bikesClient;
        }

        public List<TripDTO> Trips { get; set; } = new List<TripDTO>();
        public TripDTO TripDTO { get; set; } = new TripDTO();
        public List<ChainRotationTripsDTO> ChainRotationsTripsDTO { get; set; } = new List<ChainRotationTripsDTO>();

        public async Task<ServiceResponse<int>> AddTrip(Trip trip)
        {

            trip = await _apiClient.AddTrip(trip);

            if (trip != null)
            {
                return new ServiceResponse<int> { Message = "Trip added." };
            }
            else
            {
                return new ServiceResponse<int> { Success = false, Message = "Trip not added." };
            }

            //var result = await _http.PostAsJsonAsync("api/trip/addTrip", trip);

            //return await result.Content.ReadFromJsonAsync<ServiceResponse<int>>();
        }

        public async Task<ServiceResponse<bool>> DeleteTrip(string tripId)
        {
            var result = await _apiClient.DeleteTrip(tripId);

            if (result == true)
                return new ServiceResponse<bool> { Message = "Deleted" };
            else
                return new ServiceResponse<bool> { Success = false ,Message = "Not deleted" };

            //var result = await _http.DeleteAsync("api/Trip/" + tripId);

            //return await result.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
        }

        public async Task GetChainRotationTrips(int chainId)
        {
            throw new NotImplementedException();
            //var result = await _http.GetFromJsonAsync<ServiceResponse<List<ChainRotationTripsDTO>>>("rotationsforchain/"+chainId);

            //if (result != null && result.Data != null)
            //    ChainRotationsTripsDTO = result.Data;
        }

        public async Task  GetTripAsync(string tripId)
        {

            TripDTO = await _apiClient.GetTrip(tripId);

            //var result = await _http.GetFromJsonAsync<ServiceResponse<TripDTO>>("api/Trip/" + tripId);

            //if (result != null && result.Data != null)
            //    TripDTO = result.Data;

        }

        //gets all trips for all bikes
        public async Task GetTrips()
        {

            Trips = await _apiClient.GetTrips();
        }
        //gets all trips for a specific bike
        public async Task GetTripsForBike(int bikeId)
        {

            //var result = await _http.GetFromJsonAsync<ServiceResponse<List<TripDTO>>>("api/trip/trips/bike/"+bikeId);

            //if (result != null && result.Data != null)
            //    Trips = result.Data;
            Trips = await _apiClient.GetTripsForBike(bikeId);
        }

        public async Task<ServiceResponse<int>> UpdateTrip(Trip trip)
        {
            var result = await _apiClient.UpdateTrip(trip);

            if(result != null)
                return new ServiceResponse<int> { Message = "Updated"};
            else
            {
                return new ServiceResponse<int> { Success = false, Message = "Not updated" };
            }
                
            //var result = await _http.PutAsJsonAsync("api/trip/updateTrip", trip);

            //return await result.Content.ReadFromJsonAsync<ServiceResponse<int>>();
        }
    }
}
