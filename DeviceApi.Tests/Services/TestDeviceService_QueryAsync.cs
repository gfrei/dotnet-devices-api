using DeviceApi.Data;
using DeviceApi.Models;
using DeviceApi.Services;
using DeviceApi.Tests.Helpers;
using Moq;
using Xunit;

namespace DeviceApi.Tests.Services
{
    public class TestDeviceService_QueryAsync
    {
        
        [Fact]
        public async Task QueryAsync_TestBrandFilter()
        {
            // Arrange
            var db = TestHelpers.CreateInMemoryDb();
            var service = new DeviceService(db);
            var brand = "test";

            var device1 = TestHelpers.GetTestDevice(1, brand: brand);
            var device2 = TestHelpers.GetTestDevice(2);

            db.Devices.Add(device1);
            db.Devices.Add(device2);

            await db.SaveChangesAsync();

            // Act
            var device = await service.QueryAsync(null, brand, null);
            
            // Assert
            Assert.Single(device);
            TestHelpers.AssertEqualDevices(device.First(), device1);
        }
        
        [Fact]
        public async Task QueryAsync_TestStateFilter()
        {
            // Arrange
            var db = TestHelpers.CreateInMemoryDb();
            var service = new DeviceService(db);
            var state = "test";

            var device1 = TestHelpers.GetTestDevice(1, state: state);
            var device2 = TestHelpers.GetTestDevice(2);

            db.Devices.Add(device1);
            db.Devices.Add(device2);

            await db.SaveChangesAsync();

            // Act
            var device = await service.QueryAsync(null, null, state);
            
            // Assert
            Assert.Single(device);
            TestHelpers.AssertEqualDevices(device.First(), device1);
        }
        
        [Fact]
        public async Task QueryAsync_TestNameFilter()
        {
            // Arrange
            var db = TestHelpers.CreateInMemoryDb();
            var service = new DeviceService(db);
            var name = "test";

            var device1 = TestHelpers.GetTestDevice(1, name: name);
            var device2 = TestHelpers.GetTestDevice(2);

            db.Devices.Add(device1);
            db.Devices.Add(device2);

            await db.SaveChangesAsync();

            // Act
            var device = await service.QueryAsync(name, null, null);
            
            // Assert
            Assert.Single(device);
            TestHelpers.AssertEqualDevices(device.First(), device1);
        }
        
        [Fact]
        public async Task QueryAsync_TestAllFilters()
        {
            // Arrange
            var db = TestHelpers.CreateInMemoryDb();
            var service = new DeviceService(db);
            var val = "test";

            var device1 = TestHelpers.GetTestDevice(1, val, val, val);

            db.Devices.Add(device1);

            await db.SaveChangesAsync();

            // Act
            var device = await service.QueryAsync(val, val, val);
            
            // Assert
            Assert.Single(device);
            TestHelpers.AssertEqualDevices(device.First(), device1);
        }
        
        [Fact]
        public async Task QueryAsync_TestNoFilters()
        {
            // Arrange
            var db = TestHelpers.CreateInMemoryDb();
            var service = new DeviceService(db);

            var device1 = TestHelpers.GetTestDevice(1);

            db.Devices.Add(device1);

            await db.SaveChangesAsync();

            // Act
            var device = await service.QueryAsync(null, null, null);
            
            // Assert
            Assert.Single(device);
            TestHelpers.AssertEqualDevices(device.First(), device1);
        }
        
        [Fact]
        public async Task QueryAsync_TestNoResults()
        {
            // Arrange
            var db = TestHelpers.CreateInMemoryDb();
            var service = new DeviceService(db);
            var name = "test";

            var device1 = TestHelpers.GetTestDevice(1);

            db.Devices.Add(device1);

            await db.SaveChangesAsync();

            // Act
            var device = await service.QueryAsync(name, null, null);
            
            // Assert
            Assert.Empty(device);
        }
    }
}