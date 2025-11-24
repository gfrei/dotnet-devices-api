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
            var group = routes.MapGroup("/devices").WithTags("Devices");

            group.MapGet("/", QueryDevices)
                .WithSummary("Query devices")
                .WithDescription("""
Returns a list of devices filtered by name, brand, or state.
Query parameters:
- **name**: string — Filter by device name
- **brand**: string — Filter by brand
- **state**: string — Filter by state (available, in-use, inactive)
Examples:

GET http://localhost:8080/devices?brand=Moto
GET http://localhost:8080/devices?brand=Moto&name=Edge
GET http://localhost:8080/devices?name=Edge&state=available

""")
                .Produces<IEnumerable<Device>>(StatusCodes.Status200OK);

            group.MapGet("/{id:int}", GetDeviceById)
                .WithSummary("Get device by ID")
                .WithDescription("Returns a single device by its ID.")
                .Produces<Device>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound);

            group.MapPost("/", CreateDevice)
                .WithSummary("Create device")
                .WithDescription("Creates a new device and returns it.")
                .WithDescription("""
Creates a new device and returns it.

Examples:

POST http://localhost:8080/devices
Content-Type: application/json

{
    "name": "Edge",
    "brand": "Moto",
    "state": "available"
}
""")
                .Accepts<DeviceCreateDTO>("application/json")
                .Produces<Device>(StatusCodes.Status201Created)
                .Produces(StatusCodes.Status400BadRequest);

            group.MapPut("/{id:int}", UpdateDevice)
                .WithSummary("Update device")
                .WithDescription("Updates fields name, brand and state of an existing device.")
                .WithDescription("""
Updates fields name, brand and state of an existing device.

Examples:

PUT http://localhost:8080/devices/5
Content-Type: application/json

{
    "brand": "Samsung"
}

PUT http://localhost:8080/devices/1
Content-Type: application/json

{
    "name": "S22"
}

""")
                .Accepts<DeviceUpdateDTO>("application/json")
                .Produces<Device>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status409Conflict);
            
            group.MapDelete("/{id:int}", DeleteDeviceById)
                .WithSummary("Delete device")
                .WithDescription("Deletes a device unless it is in use.")
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status409Conflict);

            return group;
        }
        
        public static async Task<IResult> QueryDevices(IDeviceService service, string? name, string? brand, string? state)
        {
            var devices = await service.QueryAsync(name, brand, state);
            return Results.Ok(devices);
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

            return created is null
                ? Results.BadRequest($"State should be {DeviceStates.Available}, {DeviceStates.InUse}, or {DeviceStates.Inactive}.")
                : Results.Created($"/devices/{created.Id}", created);
        }

        public static async Task<IResult> UpdateDevice(IDeviceService service, int id, DeviceUpdateDTO deviceUpdateDTO)
        {
            Device update = new Device
            {
                Id = id,
                Name = deviceUpdateDTO.Name,
                Brand = deviceUpdateDTO.Brand,
                State = deviceUpdateDTO.State,
            };

            (var status, var device) = await service.UpdateAsync(id, update);

            return status switch
            {
                UpdateResult.NotFound => Results.NotFound(),
                UpdateResult.InvalidState => Results.BadRequest($"State should be {DeviceStates.Available}, {DeviceStates.InUse}, or {DeviceStates.Inactive}."),
                UpdateResult.IsInUse => Results.Conflict("Device is in use, cannot update name nor brand."),
                _ => Results.Ok(device),
            };
        }

        public static async Task<IResult> DeleteDeviceById(IDeviceService service, int id)
        {
            var result = await service.DeleteAsync(id);
            return result is DeleteResult.NotAllowed
                ? Results.Conflict("Device in use")
                : Results.NoContent();
        }
    }
}
