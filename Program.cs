global using System.Net.Http.Json;      //Have to place reference in program.cs and _imports.razor (to be seen in razor components)
global using TestFromGitToMongo.Services.httpServicExample;
global using TestFromGitToMongo.Clients;
global using TestFromGitToMongo.Services.httpclientServicExample;
global using TestFromGitToMongo.Services.TripService;
global using TestFromGitToMongo.Services.BrowserStorageService;
global using TestFromGitToMongo.Models;
global using TestFromGitToMongo.Services.ChainService;
global using TestFromGitToMongo.Services.NoteService;

using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazored.LocalStorage;

using TestFromGitToMongo;
using TestFromGitToMongo.Services.BikeService;
using TestFromGitToMongo.Services;

using TestFromGitToMongo.Data;
using TestFromGitToMongo.Services.AuthService;





var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


ConfigureServices(builder.Services);
Console.WriteLine("Base address " + builder.HostEnvironment.BaseAddress);
builder.Services.AddScoped<IBikeServiceClient, BikeServiceClient >();
//builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAuthService, AuthServiceUsingHTTPFactory>();
builder.Services.AddScoped<IBrowserStorageService, BrowserStorageService>();
builder.Services.AddScoped<ITripServiceClient, TripServiceClient>();
builder.Services.AddScoped<IChainServiceClient, ChainServiceClient>();
builder.Services.AddScoped<INoteService, NoteService>();


builder.Services.AddBlazoredLocalStorage();


await builder.Build().RunAsync();

//example of how to configure the HTTP client factory in program CS
static void ConfigureServices(IServiceCollection services)
{

    //example of Typed Client
    services.AddHttpClient<BikeAPIClient>();

    services.AddScoped<IHttpClientServiceImplementation, HttpClientFactoryService>();
}
