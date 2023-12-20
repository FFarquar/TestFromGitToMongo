
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using System.IO.IsolatedStorage;
using System.Net;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;
using TestFromGitToMongo.Models;

namespace TestFromGitToMongo.Services.AuthService
{
    public class AuthService : IAuthService
    {


        //private readonly IJSRuntime JS;
        //private readonly ILocalStorService _localStorService;
        //private IJSObjectReference? _jsModule;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IBrowserStorageService _localStorService;
        private readonly IConfiguration _configuration;
        const string authPath = @"api/auth/";


        //public AuthService(HttpClient httpClient, IBrowserStorageService localStorService)
        //{
        //    _httpClient = httpClient;
        //    _localStorService = localStorService;
        //    _httpClient.BaseAddress = new Uri(Settings.API_BaseUrl);
        //}

        //public AuthService(IBrowserStorageService localStorService, IHttpClientFactory clientFactory)
        //{

        // //   var cookieContainer = new CookieContainer();
        //    _httpClient = clientFactory.CreateClient("NewClient");
        //    _localStorService = localStorService;
        //    //_httpClient.BaseAddress = new Uri(Settings.API_BaseUrl);
        //    //JS = js;

        //}

        public AuthService(IHttpClientFactory httpClientFactory,IBrowserStorageService localStorService, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _localStorService = localStorService;
            _configuration = configuration;
        }


        public Task<ServiceResponse<bool>> ChangePassword(UserChangePassword request)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResponse<string>> Login(UserLogin request)
        {
            Console.WriteLine("Token value read from Settings.jwttoken when entering login= " + _localStorService.GetItemFromStorage("jwttoken"));

            //HttpResponseMessage result = await _httpClient.PostAsJsonAsync(authPath + "login", request);
            //var data = await result.Content.ReadAsStringAsync();

            //RegLogDTO registerResponseDTO = System.Text.Json.JsonSerializer.Deserialize<RegLogDTO>(data)!;
            //Console.WriteLine("User = " + registerResponseDTO.user);
            //Console.WriteLine("Token = " + registerResponseDTO.token);
            //Console.WriteLine("Message = " + registerResponseDTO.message);

            //_localStorService.AddItemToStorage("jwtoken", registerResponseDTO.token);


            //Console.WriteLine("Token value read from Settings.jwttoken after login = " + _localStorService.GetItemFromStorage("jwttoken"));

            return new ServiceResponse<string>();      //temp solution to check route

        }


        public async Task<ServiceResponse<int>>  Register(UserRegister request)
        {
            //Finding limitations with browser hosting. I cant extract the cookies (easily) or the headers. Spent hours and hours
            //on solutions to be able to read them.  In the end, I just added the token to the response 


            //The next 2 lines work, buit I dont get a cookie
            //            HttpResponseMessage result = await _httpClient.PostAsJsonAsync(authPath + "register", request);
            //            var data = await result.Content.ReadAsStringAsync();

            // Create the client
            string? httpClientName = _configuration["TodoHttpClientName"];
            using HttpClient client = _httpClientFactory.CreateClient(httpClientName ?? "");



            try
            {
                // Make HTTP GET request
                // Parse JSON response deserialize into Todo type


                using HttpResponseMessage result = await client.PostAsJsonAsync(authPath + "register", request);
                var data = await result.Content.ReadAsStringAsync();

                RegLogDTO registerResponseDTO = System.Text.Json.JsonSerializer.Deserialize<RegLogDTO>(data)!;
                Console.WriteLine("User = " + registerResponseDTO.user);
                Console.WriteLine("Token = " + registerResponseDTO.token);
                Console.WriteLine("Message = " + registerResponseDTO.message);

                _localStorService.AddItemToStorage("jwttoken", registerResponseDTO.token);

            }
            catch (Exception ex)
            {

            }

            return new ServiceResponse<int>();      //temp solution to check route

        }

        public async Task<ServiceResponse<int>> TestCall(UserRegister request)
        {
            //HttpResponseMessage result = await _httpClient.GetAsync("admin"); ;
            //var data = await result.Content.ReadAsStringAsync();

           return new ServiceResponse<int>();      //temp solution to check route
        }

        public async Task<ServiceResponse<bool>> ChangeUserToAdmin()
        {
            //var token = await _localStorService.GetItemFromStorage("jwttoken");
            //Console.WriteLine("Token value when entering ChangeUserToAdmin function = " + token);
            //HttpResponseMessage result = await _httpClient.GetAsync("admin");   
            //var data = await result.Content.ReadAsStringAsync();
            //Console.WriteLine(data);
            return new ServiceResponse<bool>();      //temp solution to check route
            

        }
    }

    public class RegLogDTO
    {
        public string message { get; set; }
        public string user { get; set; }
        public string token { get; set; }
    }
}
