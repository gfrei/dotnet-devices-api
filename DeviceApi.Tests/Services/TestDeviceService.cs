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
            var device = GetTestDevice();

            db.Devices.Add(device);
            await db.SaveChangesAsync();

            var service = new DeviceService(db);

            // Act
            var result = await service.GetByIdAsync(device.Id);

            // Assert
            Assert.NotNull(result);
            AssertEqualDevices(device, result);
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
            AssertEqualDevices(savedDevice, device);
        }

        // Aux
        private Device GetTestDevice()
        {
            return new Device { 
                Id = 1, 
                Name = "Edge 30",
                Brand = "Morotola",
                State = "in-use",
                CreationTime = DateTime.UtcNow };
        }

        private void AssertEqualDevices(Device deviceA, Device deviceB)
        {
            Assert.Equal(deviceA.Id, deviceB.Id);
            Assert.Equal(deviceA.Name, deviceB.Name);
            Assert.Equal(deviceA.Brand, deviceB.Brand);
            Assert.Equal(deviceA.State, deviceB.State);
            Assert.Equal(deviceA.CreationTime, deviceB.CreationTime);
        }
    }
}