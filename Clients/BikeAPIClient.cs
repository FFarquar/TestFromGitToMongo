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
        private readonly IConfiguration _config;

        //public BikeAPIClient(HttpClient client, IBrowserStorageService browserStorageService)
        //{
        //    _client = client;
        //    _browserStorageService = browserStorageService;
        //    _client.BaseAddress = new Uri(Settings.API_BaseUrl);
        //    _client.Timeout = new TimeSpan(0, 0, 30);
        //    _client.DefaultRequestHeaders.Clear();
        //    _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        //    //_config = config;

        //}

        public BikeAPIClient(HttpClient client, IBrowserStorageService browserStorageService, IConfiguration _config)
        {

            //Console.WriteLine(_config.GetValue<string>("API_Details:API_LOCATION"));
            _client = client;
            _browserStorageService = browserStorageService;
            this._config = _config;
            _client.BaseAddress = new Uri(_config["API_BaseUrl"]);
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

        public async Task<TripDTO> GetTrip(string tripId)
        {
            using (var response = await _client.GetAsync("trips/getatrip/"+tripId, HttpCompletionOption.ResponseHeadersRead))
            {
                response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();

                var Trip = await JsonSerializer.DeserializeAsync<TripDTO>(stream, _options);
                Console.WriteLine("Date returned = " + Trip.Date.ToUniversalTime());
                return Trip;

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

        public async Task<bool> DeleteTrip(string tripId)
        {
            using (var response = await _client.DeleteAsync("trips/deletetrip/" + tripId))
            {
               // response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    return true;
                else
                    return false;

                //var deleteTrip = await JsonSerializer.DeserializeAsync< DeleteTripDTO > (stream, _options);

                //if (deleteTrip != null)
                //    return deleteTrip.deleted;
                //else
                //    return false;

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

        public async Task<Trip> UpdateTrip(Trip trip)
        {
            var jsonString = JsonSerializer.Serialize(trip);

            using (var response = await _client.PatchAsync("trips/updatetrip", new StringContent(jsonString, Encoding.UTF8, "application/json")))
            {
                response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();

                var tripRes = await JsonSerializer.DeserializeAsync<Trip>(stream, _options);
                return tripRes;
            }
        }

        public async Task<List<ChainRotationTripsDTO>> GetChainRotationTrips(int chainId)
        {

            var ListOfChainRotations = await ReturnListOfChainRotationsForChain(chainId);


            if (ListOfChainRotations.Success == true && ListOfChainRotations.Data != null)
            {
                var listChainRotationTrips = new List<ChainRotationTripsDTO>();

                int i = 0;
                foreach (var trip in ListOfChainRotations.Data)
                {
                    i++;
                    var chainRotationTripsDTO = new ChainRotationTripsDTO();


                    var jsonString = JsonSerializer.Serialize(trip);

                    using (var response = await _client.GetAsync("trips/listofTripsForChainRotation/"+ chainId+"/"+i, HttpCompletionOption.ResponseHeadersRead))
                    {
                        response.EnsureSuccessStatusCode();
                        var stream = await response.Content.ReadAsStreamAsync();

                        var tripRes = await JsonSerializer.DeserializeAsync<List<TripDTO>>(stream, _options);

                        decimal totDistance = 0;
                        foreach (var trip1 in tripRes)
                        {
                            totDistance = totDistance + (decimal)trip1.TripDistance;
                        }
                        chainRotationTripsDTO.Id = i;
                        chainRotationTripsDTO.ChainId = chainId;
                        chainRotationTripsDTO.TotalDistance = totDistance;
                        chainRotationTripsDTO.ChainRotation = i;

                        chainRotationTripsDTO.Trips = tripRes;

                        listChainRotationTrips.Add(chainRotationTripsDTO);


                    }


                    ////TODO Convert this to use MONGO
                    //var query = from t in _context.Trips
                    //            join c in _context.Chains on t.ChainId equals c.Id
                    //            where t.ChainRotation == trip && t.ChainId == chainId
                    //            select new TripDTO
                    //            {
                    //                Id = t.Id,
                    //                ChainLetter = c.ChainLetter,
                    //                ChainId = t.ChainId,
                    //                Date = t.Date,
                    //                TripDistance = t.TripDistance,
                    //                ChainRotation = t.ChainRotation,
                    //                TripDescription = t.TripDescription,
                    //                TripNotes = t.TripNotes
                    //            };

                    //chainRotationTripsDTO.Id = i;
                    //chainRotationTripsDTO.ChainId = chainId;
                    //chainRotationTripsDTO.TotalDistance =
                    //    _context.Trips.Where(x => x.ChainId == chainId && x.ChainRotation == trip).Select(i => Convert.ToDecimal(i.TripDistance)).Sum();
                    //chainRotationTripsDTO.ChainRotation = trip;

                    //chainRotationTripsDTO.Trips = await query.ToListAsync();

                    //listChainRotationTrips.Add(chainRotationTripsDTO);
                }

                return listChainRotationTrips;

            }
            else
            {
                return new List<ChainRotationTripsDTO>();
            }


            return new List<ChainRotationTripsDTO>();
        }

        private async Task<ServiceResponse<List<int>>> ReturnListOfChainRotationsForChain(int chainId)
        {

            var result = new List<ChainRotationTripsDTO>();
            using (var response = await _client.GetAsync("trips/listofChainRotForChain/" + chainId, HttpCompletionOption.ResponseHeadersRead))
            {
                response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();

                result= await JsonSerializer.DeserializeAsync<List<ChainRotationTripsDTO>>(stream, _options);

            }

            if (result != null && result.Count != 0)
            {

                List<int> cr = new List<int>();

                int x = 0;
                foreach (var trip in result)
                {
                    x++;
                    //cr.Add(trip.Key);
                    cr.Add(x);
                }

                var response = new ServiceResponse<List<int>>
                {
                    Data = cr
                };

                return response;
            }
            else
            {
                return new ServiceResponse<List<int>> { Data = null, Message = "Chain not found or no rotations for this chain found." };
            }


        }

        public async Task<ChainSummaryDTO> GetChain(int chainId)
        {
            using (var response = await _client.GetAsync("chains/getchain/" + chainId, HttpCompletionOption.ResponseHeadersRead))
            {
                response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();

                var Chain = await JsonSerializer.DeserializeAsync<ChainSummaryDTO>(stream, _options);
                return Chain;

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

        public async Task<List<BikeNote>> GetListOfNotesForBike(int bikeid)
        {
            using (var response = await _client.GetAsync("notes/getnotesforabike/" + bikeid, HttpCompletionOption.ResponseHeadersRead))
            {
                response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();

                var Notes = await JsonSerializer.DeserializeAsync<List<BikeNote>>(stream, _options);
                return Notes;

            }
        }

        public async Task<BikeNote> AddNote(BikeNote note)
        {
            var jsonString = JsonSerializer.Serialize(note);

            using (var response = await _client.PostAsync("notes/addnote", new StringContent(jsonString, Encoding.UTF8, "application/json")))
            {
                response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();

                var noteRes = await JsonSerializer.DeserializeAsync<BikeNote>(stream, _options);
                return noteRes;
            }
        }

        public async Task<BikeNote> GetBikeNote(string noteId)
        {
            using (var response = await _client.GetAsync("notes/getanote/" + noteId, HttpCompletionOption.ResponseHeadersRead))
            {
                response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();

                var Note = await JsonSerializer.DeserializeAsync<BikeNote>(stream, _options);
                
                return Note;

            }
        }

        public async Task<bool> DeleteNote(string noteId)
        {
            using (var response = await _client.DeleteAsync("notes/deletenote/" + noteId))
            {

                var stream = await response.Content.ReadAsStreamAsync();

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    return true;
                else
                    return false;

            }
        }

        public async Task<BikeNote> UpdateNote(BikeNote note)
        {
            var jsonString = JsonSerializer.Serialize(note);

            using (var response = await _client.PatchAsync("notes/updatenote", new StringContent(jsonString, Encoding.UTF8, "application/json")))
            {
                response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();

                var noteRes = await JsonSerializer.DeserializeAsync<BikeNote>(stream, _options);
                return note;
            }
        }

    }

    public class RegLogDTO
    {
        public string message { get; set; }
        public string user { get; set; }
        public string token { get; set; }
    }

}
