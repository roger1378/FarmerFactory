using FarmerFactory.Web;
using FarmerFactory.Web.Components;
using FarmerFactory.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
////builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddOutputCache();

// Register API services
builder.Services.AddHttpClient<IClientService, ClientService>(client =>
{
    client.BaseAddress = new("http://localhost:5249");
});

builder.Services.AddHttpClient<IProductService, ProductService>(client =>
{
    client.BaseAddress = new("http://localhost:5249");
});

builder.Services.AddHttpClient<IPurchaseService, PurchaseService>(client =>
{
    client.BaseAddress = new("http://localhost:5249");
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.UseOutputCache();

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

////app.MapDefaultEndpoints();

await app.RunAsync();
