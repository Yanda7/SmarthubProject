#region Using Directives

using Microsoft.Extensions.DependencyInjection;
using Smarthub.Service;
using System.Net.Http.Headers;

#endregion

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

#region region HttpClient Configuration

builder.Services.AddTransient<OrderServiceRepository>();

builder.Services.AddHttpClient<OrderServiceRepository>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiBaseAddress"]);

    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
});
#endregion


builder.Services.AddControllers();


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
