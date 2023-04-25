using MvcCoreCacheRedisProductos.Helpers;
using MvcCoreCacheRedisProductos.Repositories;
using MvcCoreCacheRedisProductos.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string cnnCacheRedis = builder.Configuration.GetConnectionString("CacheRedis");
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = cnnCacheRedis;
});
builder.Services.AddSingleton<HelperPathProvider>();
builder.Services.AddTransient<RepositoryProductos>();
builder.Services.AddTransient<ServiceCacheRedis>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
