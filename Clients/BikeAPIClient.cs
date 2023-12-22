using System.Net.Http.Headers;
using System.Text;
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
            //var token = _browserStorageService.GetItemFromStorage("jwttoken");
            //Console.WriteLine("Token = " + token.Result);
            //_client.DefaultRequestHeaders.Add("'authorization", token.Result);
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

        private async Task <AuthenticationHeaderValue> AddTokenToRequest()
        {
            var token = await _browserStorageService.GetItemFromStorage("jwttoken");

            var AuthHeaderVal = new AuthenticationHeaderValue("Bearer", token);

            return AuthHeaderVal;

        }

        public async Task<ServiceResponse<bool>> TestTheAuthorizedAdminRoute()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, _client.BaseAddress + "admin");
            request.Headers.Authorization = await AddTokenToRequest();
            ServiceResponse<bool> retVal = new ServiceResponse<bool>();
            retVal.Success = false;

            using (var response = await _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
            {
                if (response.IsSuccessStatusCode)
                {
                    // Handle success
                    retVal.Success = true;
                    retVal.Message = response.StatusCode.ToString();
                }
                else
                {
                    // Handle failure
                    var responseBytes = await response.Content.ReadAsByteArrayAsync();
                    retVal.Message = response.StatusCode.ToString();

                }
            }


            return retVal;
        }

        //This is an example of how to use the sendasync method to POST and include serializied data and add auth token to header
        //The method is also saving the token to local storage.
        public async Task<ServiceResponse<string>> Login(UserLogin userLogin)
        {

            var httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.Headers.Authorization = await AddTokenToRequest();

            httpRequestMessage.Method = HttpMethod.Post;
            httpRequestMessage.RequestUri = new Uri(_client.BaseAddress + "auth/login");

            string strJson = JsonSerializer.Serialize<UserLogin>(userLogin);
            HttpContent httpContent = new StringContent(strJson, Encoding.UTF8, "application/json");
            httpRequestMessage.Content = httpContent;

            ServiceResponse<string> retVal = new ServiceResponse<string>();
            retVal.Success = false;

            using (var response = await _client.SendAsync(httpRequestMessage))
            {
                var data = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    RegLogDTO registerResponseDTO = JsonSerializer.Deserialize<RegLogDTO>(data)!;
                    _browserStorageService.AddItemToStorage("jwttoken", registerResponseDTO.token);

                    // Handle success
                    retVal.Success = true;
                    retVal.Message = registerResponseDTO.message;
                }
                else
                {
                    // Handle failure
                    RegLogDTO registerResponseDTO = JsonSerializer.Deserialize<RegLogDTO>(data)!;
                    var responseBytes = await response.Content.ReadAsByteArrayAsync();
                    retVal.Message = registerResponseDTO.message;
                }
            }

            return retVal;
        }

        //TODO: Add the register route.
        //TODO: Add the delete trip. Make it only someone who has admin access can do it. Need to change the functions to return role from DB
    }

    public class RegLogDTO
    {
        public string message { get; set; }
        public string user { get; set; }
        public string token { get; set; }
    }
}
