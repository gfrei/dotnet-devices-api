using DeviceApi.Data;
using DeviceApi.Models;
using DeviceApi.Services;
using DeviceApi.Tests.Helpers;
using Moq;
using Xunit;

public class TestDeviceService
{
    private Device GetTestDevice()
    {
        return new Device { 
            Id = 1, 
            Name = "Edge 30",
            Brand = "Morotola",
            State = "in-use",
            CreationTime = DateTime.UtcNow };
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnDevice_ThatExists()
    {
        // Arrange
        var db = TestHelpers.CreateInMemoryDb();
        var device = GetTestDevice();

        db.Devices.Add(device);
        await db.SaveChangesAsync();

        var service = new DeviceService(db);

        // Act
        var result = await service.GetByIdAsync(device.Id);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(device.Id, result.Id);
        Assert.Equal(device.Name, result.Name);
        Assert.Equal(device.Brand, result.Brand);
        Assert.Equal(device.State, result.State);
        Assert.Equal(device.CreationTime, result.CreationTime);
    }

    [Fact]
    public async Task GetByIdAsync_DeviceNotFound()
    {
        // Arrange
        var db = TestHelpers.CreateInMemoryDb();
        var device = GetTestDevice();

        db.Devices.Add(device);
        await db.SaveChangesAsync();

        var service = new DeviceService(db);

        // Act
        var result = await service.GetByIdAsync(device.Id + 1);
        
        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task CreateAsync_AddNewDevice()
    {
        // Arrange
        var db = TestHelpers.CreateInMemoryDb();
        var device = GetTestDevice();
        var service = new DeviceService(db);

        // Act
        await service.CreateAsync(device);
        
        // Assert
        var savedDevice = await db.FindAsync<Device>(device.Id);

        Assert.NotNull(savedDevice);
        Assert.Equal(savedDevice.Id, device.Id);
        Assert.Equal(savedDevice.Name, device.Name);
        Assert.Equal(savedDevice.Brand, device.Brand);
        Assert.Equal(savedDevice.State, device.State);
        Assert.Equal(device.CreationTime, device.CreationTime);
    }
}