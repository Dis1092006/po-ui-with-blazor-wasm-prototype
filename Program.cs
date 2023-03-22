using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Wasm;
using Wasm.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<IPurchaseOrderService, PurchaseOrderService>();

var poApiBaseAddress = "http://localhost:5000/api/v1/";

builder.Services.AddScoped(sp =>
{
    return new HttpClient { BaseAddress = new Uri(poApiBaseAddress) };
});

await builder.Build().RunAsync();