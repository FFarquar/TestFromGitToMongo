
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

using TestFromGitToMongo;
using TestFromGitToMongo.Services.BikeService;
using TestFromGitToMongo.Services;
using TestFromGitToMongo.Models;
using TestFromGitToMongo.Data;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");



//builder.Services.Add<DataContext>(options =>
//{
  
//});

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddScoped<IBikeServiceClient, BikeServiceClient>();


await builder.Build().RunAsync();
