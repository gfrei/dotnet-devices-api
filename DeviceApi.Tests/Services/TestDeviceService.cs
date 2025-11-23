using DeviceApi.Data;
using DeviceApi.Models;
using DeviceApi.Services;
using DeviceApi.Tests.Helpers;
using Moq;
using Xunit;

namespace DeviceApi.Tests.Services
{
    public class TestDeviceService
    {
        [Fact]
        public async Task GetByIdAsync_ShouldReturnDevice_ThatExists()
        {
            // Arrange
            var db = TestHelpers.CreateInMemoryDb();
            var device = TestHelpers.GetTestDevice();

            db.Devices.Add(device);
            await db.SaveChangesAsync();

            var service = new DeviceService(db);

            // Act
            var result = await service.GetByIdAsync(device.Id);

            // Assert
            Assert.NotNull(result);
            TestHelpers.AssertEqualDevices(device, result);
        }

        [Fact]
        public async Task GetByIdAsync_DeviceNotFound()
        {
            // Arrange
            var db = TestHelpers.CreateInMemoryDb();
            var device = TestHelpers.GetTestDevice();

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
            var device = TestHelpers.GetTestDevice();
            var service = new DeviceService(db);

            // Act
            await service.CreateAsync(device);
            
            // Assert
            var savedDevice = await db.FindAsync<Device>(device.Id);

            Assert.NotNull(savedDevice);
            TestHelpers.AssertEqualDevices(savedDevice, device);
        }

        [Fact]
        public async Task DeleteAsync_DeleteOk()
        {
            // Arrange
            var db = TestHelpers.CreateInMemoryDb();
            var device = TestHelpers.GetTestDevice();
            db.Devices.Add(device);
            await db.SaveChangesAsync();
            
            var service = new DeviceService(db);

            // Act
            bool ok = await service.DeleteAsync(device.Id);
            
            // Assert
            var deletedDevice = await db.FindAsync<Device>(device.Id);

            Assert.True(ok);
            Assert.Null(deletedDevice);
        }
    }
}