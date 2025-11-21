using DeviceApi.Data;
using DeviceApi.Dtos;
using DeviceApi.Models;
using DeviceApi.Services;

namespace DeviceApi.Endpoints
{
    public static class DeviceEndpoints
    {
        public static RouteGroupBuilder MapDeviceEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/devices");

            group.MapGet("/{id:int}", GetDeviceById);
            group.MapPost("/", CreateDevice);

            return group;
        }

        public static async Task<IResult> GetDeviceById(IDeviceService service, int id)
        {
            var device = await service.GetByIdAsync(id);

            return device is not null 
                ? Results.Ok(device)
                : Results.NotFound();
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
            
            return Results.Created($"/devices/{created.Id}", created);
        }
    }
}