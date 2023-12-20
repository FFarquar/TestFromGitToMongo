using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using TestFromGitToMongo.Models;
using TestFromGitToMongo.Services.AuthService;
using TestFromGitToMongo.Services.BrowserStorageService;

namespace TestFromGitToMongo.Data
{
    public class AddHeadersDelegatingHandler : DelegatingHandler
    {
        private readonly IBrowserStorageService _browserStorageService;

        public AddHeadersDelegatingHandler(IBrowserStorageService browserStorageService) : base(new HttpClientHandler())
        {
            _browserStorageService = browserStorageService;
        }


        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            
            //TODO : have to add singleton here
            Console.WriteLine("JW Token added to header in sendasync = " + _browserStorageService.GetItemFromStorage("jwttoken"));
            var token = _browserStorageService.GetItemFromStorage("jwttoken");
            request.Headers.Add("jwttoken", token.Result);  // Add whatever headers you want here

            request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
            return base.SendAsync(request, cancellationToken);
        }
    }

}
