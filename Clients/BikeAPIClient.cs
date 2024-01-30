using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TestFromGitToMongo.Clients
{
    //Example of a typed client
    public class BooleanConverter : JsonConverter<bool>
    {

        public override bool Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options) =>
            bool.Parse(reader.GetString());

        public override void Write(
            Utf8JsonWriter writer,
            bool b,
            JsonSerializerOptions options) =>
            writer.WriteStringValue(b.ToString().ToLower());
    }

    public class BikeAPIClient
    {
        public HttpClient _client { get; }
        private readonly JsonSerializerOptions _options;
        private readonly IBrowserStorageService _browserStorageService;
        private readonly IConfiguration _config;
        private readonly GlobalVariables _globalVariables;



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

        public BikeAPIClient(HttpClient client, IBrowserStorageService browserStorageService, IConfiguration _config, GlobalVariables globalVariables)
        {

            //Console.WriteLine(_config.GetValue<string>("API_Details:API_LOCATION"));
            _client = client;
            _browserStorageService = browserStorageService;
            this._config = _config;
            _globalVariables = globalVariables;
            _client.BaseAddress = new Uri(_config["API_BaseUrl"]);
            _client.Timeout = new TimeSpan(0, 0, 30);
            _client.DefaultRequestHeaders.Clear();
            //_options = new JsonSerializerOptions
            //{
            //    PropertyNameCaseInsensitive = true,
            //    Converters = { new BooleanConverter() }
            //};
            _options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<List<Bike>> Bikes_GetAll()
        {
            //using (var response = await _client.GetAsync("bikes", HttpCompletionOption.ResponseHeadersRead))
            //{
            //    response.EnsureSuccessStatusCode();
            //    var stream = await response.Content.ReadAsStreamAsync();
            //    var Bikes  = await JsonSerializer.DeserializeAsync<List<Bike>>(stream, _options);
            //    return Bikes;
            //}
            var request = new HttpRequestMessage(HttpMethod.Get, _client.BaseAddress + "bikes");
            request.Headers.Authorization = await Auth_AddTokenToRequest();

            using (var response = await _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
            {
                if (response.IsSuccessStatusCode)
                {
                    // Handle success
                    var stream = await response.Content.ReadAsStreamAsync();

                    var Bikes = await JsonSerializer.DeserializeAsync<List<Bike>>(stream, _options);
                    return Bikes;
                }
                else
                {
                    // Handle failure. Empty list of bikes
                    return new List<Bike>();
                }
            }
        }

        public async Task<Bike> GetBike(int bikeId)
        {
            using (var response = await _client.GetAsync("bikes/bike/" + bikeId, HttpCompletionOption.ResponseHeadersRead))
            {
                response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();

                stream.Position = 0;        //Deserialize doesent work without this

                var bike = await JsonSerializer.DeserializeAsync<Bike>(stream, _options);
                return bike;

            }
        }

        public async Task<TripDTO> Trip_Get(string tripId)
        {

            var request = new HttpRequestMessage(HttpMethod.Get, _client.BaseAddress + "trips/getatrip/" + tripId);
            request.Headers.Authorization = await Auth_AddTokenToRequest();

            using (var response = await _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
            {
                if (response.IsSuccessStatusCode)
                {
                    // Handle success
                    var stream = await response.Content.ReadAsStreamAsync();

                    var Trip = await JsonSerializer.DeserializeAsync<TripDTO>(stream, _options);
                    return Trip;
                }
                else
                {
                    // Handle failure. Empty list of trips
                    return new TripDTO();
                }
            }
        }

        public async Task<List<TripDTO>> Trip_GetAllTrips()
        {

            var request = new HttpRequestMessage(HttpMethod.Get, _client.BaseAddress + "trips/getalltrips");
            request.Headers.Authorization = await Auth_AddTokenToRequest();

            using (var response = await _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
            {
                if (response.IsSuccessStatusCode)
                {
                    // Handle success
                    var stream = await response.Content.ReadAsStreamAsync();

                    var Trips = await JsonSerializer.DeserializeAsync<List<TripDTO>>(stream, _options);
                    return Trips;
                }
                else
                {
                    // Handle failure. Empty list of trips
                    return new List<TripDTO>();
                }
            }
        }

        public async Task<List<TripDTO>> Trip_GetTripsForBike(int bikeId)
        {
            //using (var response = await _client.GetAsync("trips/gettripsforbike/"+bikeId, HttpCompletionOption.ResponseHeadersRead))
            //{
            //    response.EnsureSuccessStatusCode();
            //    var stream = await response.Content.ReadAsStreamAsync();

            //    var Trips = await JsonSerializer.DeserializeAsync<List<TripDTO>>(stream, _options);
            //    return Trips;

            //}

            var request = new HttpRequestMessage(HttpMethod.Get, _client.BaseAddress + "trips/gettripsforbike/" + bikeId);
            request.Headers.Authorization = await Auth_AddTokenToRequest();

            using (var response = await _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
            {
                if (response.IsSuccessStatusCode)
                {
                    // Handle success
                    var stream = await response.Content.ReadAsStreamAsync();

                    var Trips = await JsonSerializer.DeserializeAsync<List<TripDTO>>(stream, _options);
                    return Trips;
                }
                else
                {
                    // Handle failure. Empty list of trips
                    return new List<TripDTO>();
                }
            }
        }

        public async Task<bool> Trip_Delete(string tripId)
        {

            var request = new HttpRequestMessage(HttpMethod.Delete, _client.BaseAddress + "trips/deletetrip/" + tripId);
            request.Headers.Authorization = await Auth_AddTokenToRequest();

            using (var response = await _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
            {
                if (response.IsSuccessStatusCode)
                {
                    // Handle success
                    var stream = await response.Content.ReadAsStreamAsync();


                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        return true;
                    else
                        return false;
                }
                else
                {
                    // Handle failure
                    return false;
                }
            }
            //using (var response = await _client.DeleteAsync("trips/deletetrip/" + tripId))
            //{
            //   // response.EnsureSuccessStatusCode();
            //    var stream = await response.Content.ReadAsStreamAsync();

            //    if (response.StatusCode == System.Net.HttpStatusCode.OK)
            //        return true;
            //    else
            //        return false;
            //}
        }

        public async Task<Trip> Trip_Add(Trip trip)
        {
            var jsonString = JsonSerializer.Serialize(trip);

            var request = new HttpRequestMessage(HttpMethod.Post, _client.BaseAddress + "trips/addtrip");
            request.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            request.Headers.Authorization = await Auth_AddTokenToRequest();

            using (var response = await _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
            {
                if (response.IsSuccessStatusCode)
                {
                    // Handle success
                    var stream = await response.Content.ReadAsStreamAsync();

                    var Trip = await JsonSerializer.DeserializeAsync<Trip>(stream, _options);
                    return Trip;
                }
                else
                {
                    // Handle failure. Empty trip
                    return new Trip();
                }
            }

            //using (var response = await _client.PostAsync("trips/addtrip", new StringContent(jsonString, Encoding.UTF8, "application/json")))
            //{
            //    response.EnsureSuccessStatusCode();
            //    var stream = await response.Content.ReadAsStreamAsync();

            //    var tripRes = await JsonSerializer.DeserializeAsync<Trip>(stream, _options);
            //    return tripRes;
            //}
        }

        public async Task<Trip> Trip_Update(Trip trip)
        {
            var jsonString = JsonSerializer.Serialize(trip);

            var request = new HttpRequestMessage(HttpMethod.Patch, _client.BaseAddress + "trips/updatetrip");
            request.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            request.Headers.Authorization = await Auth_AddTokenToRequest();

            using (var response = await _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
            {
                if (response.IsSuccessStatusCode)
                {
                    // Handle success
                    var stream = await response.Content.ReadAsStreamAsync();

                    var Trip = await JsonSerializer.DeserializeAsync<Trip>(stream, _options);
                    return Trip;
                }
                else
                {
                    // Handle failure. Empty trip
                    return new Trip();
                }
            }

            //using (var response = await _client.PatchAsync("trips/updatetrip", new StringContent(jsonString, Encoding.UTF8, "application/json")))
            //{
            //    response.EnsureSuccessStatusCode();
            //    var stream = await response.Content.ReadAsStreamAsync();

            //    var tripRes = await JsonSerializer.DeserializeAsync<Trip>(stream, _options);
            //    return tripRes;
            //}
        }

        public async Task<List<ChainRotationTripsDTO>> Trip_GetChainRotations(int chainId)
        {

            var ListOfChainRotations = await Trip_ReturnListOfChainRotationsForChain(chainId);


            if (ListOfChainRotations.Success == true && ListOfChainRotations.Data != null)
            {
                var listChainRotationTrips = new List<ChainRotationTripsDTO>();

                int i = 0;
                foreach (var trip in ListOfChainRotations.Data)
                {
                    i++;
                    //var chainRotationTripsDTO = new ChainRotationTripsDTO();

                    //var jsonString = JsonSerializer.Serialize(trip);


                    //    using (var response = await _client.GetAsync("trips/listofTripsForChainRotation/"+ chainId+"/"+i, HttpCompletionOption.ResponseHeadersRead))
                    //    {
                    //        response.EnsureSuccessStatusCode();
                    //        var stream = await response.Content.ReadAsStreamAsync();

                    //        var tripRes = await JsonSerializer.DeserializeAsync<List<TripDTO>>(stream, _options);

                    //        decimal totDistance = 0;
                    //        foreach (var trip1 in tripRes)
                    //        {
                    //            totDistance = totDistance + (decimal)trip1.TripDistance;
                    //        }
                    //        chainRotationTripsDTO.Id = i;
                    //        chainRotationTripsDTO.ChainId = chainId;
                    //        chainRotationTripsDTO.TotalDistance = totDistance;
                    //        chainRotationTripsDTO.ChainRotation = i;

                    //        chainRotationTripsDTO.Trips = tripRes;

                    //        listChainRotationTrips.Add(chainRotationTripsDTO);

                    //    }
                    //}

                    var chainRotationTripsDTO = new ChainRotationTripsDTO();
                    var request = new HttpRequestMessage(HttpMethod.Get, _client.BaseAddress + "trips/listofTripsForChainRotation/" + chainId + "/" + i);
                    request.Headers.Authorization = await Auth_AddTokenToRequest();
                    using (var response = await _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
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
                }

                return listChainRotationTrips;
            }
            else
            {
                return new List<ChainRotationTripsDTO>();
            }
            return new List<ChainRotationTripsDTO>();
        }

        private async Task<ServiceResponse<List<int>>> Trip_ReturnListOfChainRotationsForChain(int chainId)
        {

            //var result = new List<ChainRotationTripsDTO>();
            //using (var response = await _client.GetAsync("trips/listofChainRotForChain/" + chainId, HttpCompletionOption.ResponseHeadersRead))
            //{
            //    response.EnsureSuccessStatusCode();
            //    var stream = await response.Content.ReadAsStreamAsync();

            //    result= await JsonSerializer.DeserializeAsync<List<ChainRotationTripsDTO>>(stream, _options);

            //}
            var result = new List<ChainRotationTripsDTO>();
            var request = new HttpRequestMessage(HttpMethod.Get, _client.BaseAddress + "trips/listofChainRotForChain/" + chainId);
            request.Headers.Authorization = await Auth_AddTokenToRequest();

            using (var response = await _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
            {
                if (response.IsSuccessStatusCode)
                {
                    // Handle success
                    var stream = await response.Content.ReadAsStreamAsync();

                    result = await JsonSerializer.DeserializeAsync<List<ChainRotationTripsDTO>>(stream, _options);
                }
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

        private async Task<AuthenticationHeaderValue> Auth_AddTokenToRequest()
        {
            //instead of storing the token in the local storage, which is a bad idea, the token will be stored in memory.
            //this means that when you close the browser, the token will be lost. The user will need to log in each time they
            //connect to the browser
            //var token = await _browserStorageService.GetItemFromStorage("jwttoken");
            //var AuthHeaderVal = new AuthenticationHeaderValue("Bearer", token);

            var AuthHeaderVal = new AuthenticationHeaderValue("Bearer", _globalVariables.JWTToken);

            return AuthHeaderVal;

        }

        public async Task<ServiceResponse<bool>> TestTheAuthorizedAdminRoute()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, _client.BaseAddress + "admin");
            request.Headers.Authorization = await Auth_AddTokenToRequest();
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
        public async Task<ServiceResponse<string>> Auth_Login(UserLogin userLogin)
        {

            var httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.Headers.Authorization = await Auth_AddTokenToRequest();

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

                    _globalVariables.JWTToken = registerResponseDTO.token;
                    //_browserStorageService.AddItemToStorage("jwttoken", registerResponseDTO.token);

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

        public async Task<List<BikeNote>> Note_GetListForBike(int bikeid)
        {

            //This works when the route on the API is not protected
            //using (var response = await _client.GetAsync("notes/getnotesforabike/" + bikeid, HttpCompletionOption.ResponseHeadersRead))
            //{
            //    response.EnsureSuccessStatusCode();
            //    var stream = await response.Content.ReadAsStreamAsync();

            //    var Notes = await JsonSerializer.DeserializeAsync<List<BikeNote>>(stream, _options);
            //    return Notes;

            //}

            var request = new HttpRequestMessage(HttpMethod.Get, _client.BaseAddress + "notes/getnotesforabike/" + bikeid);
            request.Headers.Authorization = await Auth_AddTokenToRequest();

            using (var response = await _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
            {
                if (response.IsSuccessStatusCode)
                {
                    // Handle success
                    var stream = await response.Content.ReadAsStreamAsync();

                    var Notes = await JsonSerializer.DeserializeAsync<List<BikeNote>>(stream, _options);
                    return Notes;
                }
                else
                {
                    // Handle failure. Empty list of notes
                    return new List<BikeNote>();
                }
            }

        }

        public async Task<ServiceResponse<BikeNote>> Note_Add(BikeNote note)
        {
            var jsonString = JsonSerializer.Serialize(note);

            var request = new HttpRequestMessage(HttpMethod.Post, _client.BaseAddress + "notes/addnote");
            request.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            request.Headers.Authorization = await Auth_AddTokenToRequest();

            ServiceResponse<BikeNote> retResponse = new ServiceResponse<BikeNote>();
            using (var response = await _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
            {
                if (response.IsSuccessStatusCode)
                {
                    // Handle success
                    var stream = await response.Content.ReadAsStreamAsync();



                    var noteRes = await JsonSerializer.DeserializeAsync<BikeNote>(stream, _options);
                    retResponse.Data = noteRes;
                }
                else
                {
                    // Handle failure. Empty note
                    retResponse.Data = new BikeNote();
                    retResponse.Success = false;
                    retResponse.Message = response.StatusCode.ToString();
                }
            }

            return retResponse;
        }

        public async Task<BikeNote> Note_GetNote(string noteId)
        {
            //using (var response = await _client.GetAsync("notes/getanote/" + noteId, HttpCompletionOption.ResponseHeadersRead))
            //{
            //    response.EnsureSuccessStatusCode();
            //    var stream = await response.Content.ReadAsStreamAsync();

            //    var Note = await JsonSerializer.DeserializeAsync<BikeNote>(stream, _options);

            //    return Note;

            //}
            var request = new HttpRequestMessage(HttpMethod.Get, _client.BaseAddress + "notes/getanote/" + noteId);
            request.Headers.Authorization = await Auth_AddTokenToRequest();

            using (var response = await _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
            {
                if (response.IsSuccessStatusCode)
                {
                    // Handle success
                    var stream = await response.Content.ReadAsStreamAsync();

                    var Note = await JsonSerializer.DeserializeAsync<BikeNote>(stream, _options);
                    return Note;
                }
                else
                {
                    // Handle failure. Empty note
                    return new BikeNote();
                }
            }
        }

        public async Task<ServiceResponse<bool>> Note_Delete(string noteId)
        {
            //if there are attachments for this note, they have to be deleted too

            BikeNote bn = await Note_GetNote(noteId);

            if (bn.UploadResult.Count != 0)
            {
                List<FileDetail> filesToDelete = new List<FileDetail>();
                foreach (var ur in bn.UploadResult)
                {
                    FileDetail fd = new FileDetail()
                    {
                        OriginalFileName = ur.FileName,
                        ServerPath = ur.ServerPath
                    };
                    filesToDelete.Add(fd);
                }

                var responsetoDelete = await Attachment_Delete(filesToDelete);

                if (responsetoDelete.Success == false)
                {
                    return new ServiceResponse<bool>()
                    {
                        Success = false,
                        Message = "Attachment(s) for this note couldn't be deleted. The note has not been deleted either"
                    };
                }
            }

            var request = new HttpRequestMessage(HttpMethod.Delete, _client.BaseAddress + "notes/deletenote/" + noteId);
            request.Headers.Authorization = await Auth_AddTokenToRequest();

            ServiceResponse<bool> retResponse = new ServiceResponse<bool>();

            using (var response = await _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
            {
                if (response.IsSuccessStatusCode)
                {
                    // Handle success
                    retResponse.Data = true;
                    retResponse.Success = true;
                    retResponse.Message = "Note deleted";
                }
                else
                {
                    // Handle failure. Empty note
                    retResponse.Data = false;
                    retResponse.Success = false;
                    //var stream = await response.Content.ReadAsStringAsync();


                    retResponse.Message = response.StatusCode.ToString();

                }
            }

            return retResponse;
        }

        //public async Task<bool> DeleteNote(string noteId)
        //{


        //    //using (var response = await _client.DeleteAsync("notes/deletenote/" + noteId))
        //    //{

        //    //    var stream = await response.Content.ReadAsStreamAsync();

        //    //    if (response.StatusCode == System.Net.HttpStatusCode.OK)
        //    //        return true;
        //    //    else
        //    //        return false;

        //    //}
        //}

        public async Task<ServiceResponse<BikeNote>> Note_Update(BikeNote note)
        {
            var jsonString = JsonSerializer.Serialize(note);

            //using (var response = await _client.PatchAsync("notes/updatenote", new StringContent(jsonString, Encoding.UTF8, "application/json")))
            //{
            //    response.EnsureSuccessStatusCode();
            //    var stream = await response.Content.ReadAsStreamAsync();

            //    var noteRes = await JsonSerializer.DeserializeAsync<BikeNote>(stream, _options);
            //    return note;
            //}

            var request = new HttpRequestMessage(HttpMethod.Patch, _client.BaseAddress + "notes/updatenote");
            request.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            request.Headers.Authorization = await Auth_AddTokenToRequest();

            ServiceResponse<BikeNote> retResponse = new ServiceResponse<BikeNote>();

            using (var response = await _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
            {
                if (response.IsSuccessStatusCode)
                {
                    // Handle success
                    var stream = await response.Content.ReadAsStreamAsync();

                    var Note = await JsonSerializer.DeserializeAsync<BikeNote>(stream, _options);
                    retResponse.Data = Note;
                }
                else
                {
                    // Handle failure. Empty note
                    retResponse.Success = false;
                    retResponse.Data = new BikeNote();

                    retResponse.Message = response.StatusCode.ToString();
                }
            }
            return retResponse;
        }

        public async Task<ServiceResponse<List<UploadResult>>> Attachment_Add(MultipartFormDataContent content)
        {
            //var jsonString = JsonSerializer.Serialize(trip);

            var request = new HttpRequestMessage(HttpMethod.Post, _client.BaseAddress + "images/upload");
            //request.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            request.Content = content;
            request.Headers.Authorization = await Auth_AddTokenToRequest();

            using (var response = await _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
            {
                if (response.IsSuccessStatusCode)
                {

                    // Handle success
                    var stream = await response.Content.ReadAsStreamAsync();

                    var uploadResults = await JsonSerializer.DeserializeAsync<UploadResponseObj>(stream, _options);
                    ServiceResponse<List<UploadResult>> srResult = new ServiceResponse<List<UploadResult>>();

                    srResult.Data = uploadResults.UploadResults;
                    return srResult;
                }
                else
                {
                    // Handle failure. Empty attachment result.
                    ServiceResponse<List<UploadResult>> srResultFail = new ServiceResponse<List<UploadResult>>();
                    srResultFail.Success = false;
                    srResultFail.Message = "Files not uploaded";
                    return srResultFail;
                }
            }
        }

        //public async Task<ServiceResponse<List<bool>>> Attachment_Delete(List<FileDetail> filesToDelete)
        //{

        //    ServiceResponse<List<bool>> FileDeleteResults = new ServiceResponse<List<bool>>();
        //    List<bool> fileDeletions = new List<bool>();
        //    foreach (var file in filesToDelete)
        //    {
        //        string[] path = file.ServerPath.Split("/");
        //        var request = new HttpRequestMessage(HttpMethod.Delete, _client.BaseAddress + "images/delete/" + path[0] + "/" + path[1] + "/" + file.OriginalFileName);
        //        request.Headers.Authorization = await Auth_AddTokenToRequest();

        //        using (var response = await _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
        //        {
        //            if (response.IsSuccessStatusCode)
        //            {
        //                fileDeletions.Add(true);
        //            }
        //            else
        //            {
        //                fileDeletions.Add(false);
        //                FileDeleteResults.Success = false;
        //                FileDeleteResults.Message = FileDeleteResults.Message + ".  " + file.OriginalFileName + "  wasnt deleted";
        //            }
        //        }

        //        FileDeleteResults.Data = fileDeletions;
        //    }

        //    return FileDeleteResults;
        //}

        public async Task<ServiceResponse<List<bool>>> Attachment_Delete(List<FileDetail> filesToDelete)
        {

            ServiceResponse<List<bool>> FileDeleteResults = new ServiceResponse<List<bool>>();
            List<bool> fileDeletions = new List<bool>();
            foreach (var file in filesToDelete)
            {
                //string[] path = file.ServerPath.Split("/");
                string path = file.ServerPath.Replace("/", ",");
                var request = new HttpRequestMessage(HttpMethod.Delete, _client.BaseAddress + "images/delete/" + path + "/" + file.OriginalFileName);
                request.Headers.Authorization = await Auth_AddTokenToRequest();

                using (var response = await _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        fileDeletions.Add(true);
                    }
                    else
                    {
                        fileDeletions.Add(false);
                        FileDeleteResults.Success = false;
                        FileDeleteResults.Message = FileDeleteResults.Message + ".  " + file.OriginalFileName + "  wasnt deleted";
                    }
                }

                FileDeleteResults.Data = fileDeletions;
            }

            return FileDeleteResults;
        }

        public async Task<ServiceResponse<DotNetStreamReference>> Attachment_GeFile(string storedPath, string fileName)
        {
            //string[] path = storedPath.Split("/");
            string path = storedPath.Replace("/", ",");

            _client.DefaultRequestHeaders.Authorization = await Auth_AddTokenToRequest();
            //var imageStreamRes = await _client.GetAsync(_client.BaseAddress + "images/getafile/" + path[0] + "/" + path[1] + "/" + fileName);
            var imageStreamRes = await _client.GetAsync(_client.BaseAddress + "images/getafile/" + path + "/" + fileName);
            if (imageStreamRes is not null)
            {
                return new ServiceResponse<DotNetStreamReference>
                {
                    Data = new DotNetStreamReference(imageStreamRes.Content.ReadAsStream())
                };
            }
            else
            {
                return new ServiceResponse<DotNetStreamReference>
                {
                    Success = false,
                    Message = "Some issue getting file"
                };
            }


            //this is the original request, that doesnt work
            //using (var response = await _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
            //{
            //    if (response is not null)
            //    {

            //    //TODO: THE JS Interop cant render the file not sure if the file needs to be read as a memory stream
            //    //read this https://learn.microsoft.com/en-us/aspnet/core/blazor/file-downloads?view=aspnetcore-8.0
            //        ServiceResponse<DotNetStreamReference> returnVal = new ServiceResponse<DotNetStreamReference>();

            //        returnVal.Data = new DotNetStreamReference(await response.Content.ReadAsStreamAsync());
            //        return returnVal;

            //    }
            //    else
            //    {
            //        return new ServiceResponse<DotNetStreamReference>
            //        {
            //            Success = false,
            //            Message = "Some issue getting file"
            //        };
            //    }
            //}
        }
    }

    //Object to desiarlize the response from an upload
    public class UploadResponseObj
    {
        public List<UploadResult> UploadResults { get; set; }
    }

    public class RegLogDTO
    {
        public string message { get; set; }
        public string user { get; set; }
        public string token { get; set; }
    }

    //public class MessageResponse_General
    //{
    //    public string message { get; set; }
    //}

}
