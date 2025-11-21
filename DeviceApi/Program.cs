using DeviceApi.Data;
using DeviceApi.Dtos;
using DeviceApi.Models;
using DeviceApi.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddScoped<IDeviceService, DeviceService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();


app.MapPost("/devices/", async (IDeviceService service, DeviceCreateDTO deviceCreateDTO) =>
{
    DateTime localDateTime = DateTime.Now;
    DateTime utcDateTime = localDateTime.ToUniversalTime();

    Device device = new Device
    {
        Name = deviceCreateDTO.Name,
        Brand = deviceCreateDTO.Brand,
        State = deviceCreateDTO.State,
        CreationTime = utcDateTime,
    };

    await service.CreateAsync(device);
    
    return Results.Created($"/devices/{device.Id}", device);
});

app.MapGet("/devices/{id:int}", async (IDeviceService service, int id) =>
{
    var device = await service.GetByIdAsync(id);

    return device is not null 
        ? Results.Ok(device)
        : Results.NotFound();
});


app.Run();
