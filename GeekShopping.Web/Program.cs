using GeekShopping.Web.Services;
using GeekShopping.Web.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// iniciar uma var com os parametros do appsettings
var appSettings = builder.Configuration;

// Add services to the container.
builder.Services.AddControllersWithViews();

var productUrl = builder.Configuration["ServicesUrls:ProductAPI"];

builder.Services.AddHttpClient<IProductServices, ProductService>(
    c => c.BaseAddress = new Uri(productUrl));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
