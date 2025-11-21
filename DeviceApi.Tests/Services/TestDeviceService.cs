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

        [Fact]
        public async Task GetAllAsync_NoData()
        {
            // Arrange
            var db = TestHelpers.CreateInMemoryDb();
            var service = new DeviceService(db);

            // Act
            var devices = await service.GetAllAsync();
            
            // Assert
            Assert.NotNull(devices);
            Assert.Empty(devices);
        }
        
        [Fact]
        public async Task GetAllAsync_SingleEntry()
        {
            // Arrange
            var db = TestHelpers.CreateInMemoryDb();
            var service = new DeviceService(db);
            var device = GetTestDevice();

            db.Devices.Add(device);
            await db.SaveChangesAsync();

            // Act
            var devices = await service.GetAllAsync();
            
            // Assert
            Assert.NotNull(devices);
            Assert.Single(devices);
            
            var returned = devices.First();
            AssertEqualDevices(device, returned);
        }
        
        [Fact]
        public async Task GetAllAsync_ManyEntries()
        {
            // Arrange
            var db = TestHelpers.CreateInMemoryDb();
            var service = new DeviceService(db);

            var device1 = GetTestDevice(1);
            var device2 = GetTestDevice(2);
            var device3 = GetTestDevice(3);

            db.Devices.Add(device1);
            db.Devices.Add(device2);
            db.Devices.Add(device3);

            await db.SaveChangesAsync();

            // Act
            var devices = await service.GetAllAsync();
            
            // Assert
            Assert.NotNull(devices);
            Assert.Equal(3, devices.Count());
        }

        // Aux
        private Device GetTestDevice(int id = 1)
        {
            return new Device {
                Id = id,
                Name = "name",
                Brand = "brand",
                State = "state",
                CreationTime = DateTime.UtcNow
            };
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