#region Using directives
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Smarthub.API.Data;
using Smarthub.API.Models;
using Smarthub.API.Repository;
using Smarthub.API.Services;
using System.Text;
using static Smarthub.API.Services.IUnitOfWork;

#endregion

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

});

builder.Services.AddScoped<IUnitOfWork<Order>, OrderRepository>();

builder.Services.AddScoped<IUnitOfWork<OrderLine>, OrderLineRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
