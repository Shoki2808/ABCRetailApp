using ABCRetailApp.Components;
using ABCRetailApp.Data;
using ABCRetailApp.Services;
using Azure.Identity;
using Azure.Storage.Blobs;
using Radzen;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddRazorPages();
builder.Services.AddRadzenComponents();
builder.Configuration.AddJsonFile("appsettings.json");
// Configuration
var cfg = builder.Configuration.GetSection("Azure");


// Register Azure Table repository
builder.Services.AddSingleton<IOrderRepository>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var connectionString = cfg["tables:connectionString"];
    var tableName = cfg["tables:orderTable"];

    var repo = new TableOrderRepository(connectionString!, tableName!);
    repo.InitializeAsync().GetAwaiter().GetResult(); // Ensure table exists at startup
    return repo;
});
// Register Azure Table repository
builder.Services.AddSingleton<IProductRepository>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var connectionString = cfg["tables:connectionString"];
    var tableName = cfg["tables:productTable"];

    var repo = new ProductRepository(connectionString!, tableName!);
    repo.InitializeAsync().GetAwaiter().GetResult(); // Ensure table exists at startup
    return repo;
});


var storageConn = cfg.GetSection("Storage")["ConnectionString"];
var blobServiceClient = !string.IsNullOrWhiteSpace(storageConn)
    ? new BlobServiceClient(storageConn)
    : new BlobServiceClient(new Uri($"https://{Environment.GetEnvironmentVariable("abcretailblost10480270")}.blob.core.windows.net"), new DefaultAzureCredential());

builder.Services.AddSingleton(blobServiceClient);
builder.Services.AddSingleton<IBlobStorageService>(sp =>
    new BlobStorageService(sp.GetRequiredService<BlobServiceClient>(), cfg.GetSection("Storage")["Container"]!));

// Domain services
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IProductService, ProductService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
