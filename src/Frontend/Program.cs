using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Frontend;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var gatewayBaseUrl = builder.Configuration["GatewayApi:BaseUrl"] ?? "http://localhost:5204";

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(gatewayBaseUrl)
});

await builder.Build().RunAsync();