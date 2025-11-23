using DeviceApi.Data;
using DeviceApi.Dtos;
using DeviceApi.Models;
using DeviceApi.Services;

namespace DeviceApi.Handlers
{
    public static class DeviceHandlers
    {
        public static RouteGroupBuilder MapDeviceEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/devices");

            group.MapGet("/", QueryDevices);
            group.MapGet("/{id:int}", GetDeviceById);

            group.MapPost("/", CreateDevice);
            
            group.MapDelete("/{id:int}", DeleteDeviceById);

            return group;
        }

        public static async Task<IResult> GetDeviceById(IDeviceService service, int id)
        {
            var device = await service.GetByIdAsync(id);

            return device is not null 
                ? Results.Ok(device)
                : Results.NotFound();
        }

        public static async Task<IResult> DeleteDeviceById(IDeviceService service, int id)
        {
            var result = await service.DeleteAsync(id);
            return result is DeleteResult.Deleted
                ? Results.NoContent()
                : Results.Conflict("Device in use");
        }
        
        public static async Task<IResult> QueryDevices(IDeviceService service, string? name, string? brand, string? state)
        {
            var devices = await service.QueryAsync(name, brand, state);
            return Results.Ok(devices);
        }
        
        public static async Task<IResult> CreateDevice(IDeviceService service, DeviceCreateDTO deviceCreateDTO)
        {
            Device device = new Device
            {
                Name = deviceCreateDTO.Name,
                Brand = deviceCreateDTO.Brand,
                State = deviceCreateDTO.State,
                CreationTime = DateTime.UtcNow,
            };

            var created = await service.CreateAsync(device);

            return created is null
                ? Results.BadRequest($"State should be {DeviceStates.Available}, {DeviceStates.InUse}, or {DeviceStates.Inactive},")
                : Results.Created($"/devices/{created.Id}", created);
        }
    }
}