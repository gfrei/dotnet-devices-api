using DeviceApi.Data;
using DeviceApi.Dtos;
using DeviceApi.Models;
using DeviceApi.Services;
using DeviceApi.Tests.Helpers;
using Moq;
using Xunit;

namespace DeviceApi.Tests.Services
{
    public class TestDeviceService_UpdateAsync
    {
        [Fact]
        public async Task UpdateAsync_UpdateName_Ok()
        {
            // Arrange
            var db = TestHelpers.CreateInMemoryDb();
            var service = new DeviceService(db);

            var device = TestHelpers.GetTestDevice();
            db.Devices.Add(device);
            
            await db.SaveChangesAsync();

            var update = TestHelpers.GetEmptyTestDevice(name: "new_name");

            // Act
            (var result, var updated) = await service.UpdateAsync(device.Id, update);
            
            // Assert
            Assert.Equal(UpdateResult.Updated, result);

            Assert.NotNull(updated);
            Assert.Equal(update.Name, updated.Name);

            Assert.Equal(device.Brand, updated.Brand);
            Assert.Equal(device.State, updated.State);
            Assert.Equal(device.CreationTime, updated.CreationTime);
            Assert.Equal(device.Id, updated.Id);
        }

        [Fact]
        public async Task UpdateAsync_UpdateBrand_Ok()
        {
            // Arrange
            var db = TestHelpers.CreateInMemoryDb();
            var service = new DeviceService(db);

            var device = TestHelpers.GetTestDevice();
            db.Devices.Add(device);
            
            await db.SaveChangesAsync();

            var update = TestHelpers.GetEmptyTestDevice(brand: "new_brand");

            // Act
            (var result, var updated) = await service.UpdateAsync(device.Id, update);
            
            // Assert
            Assert.Equal(UpdateResult.Updated, result);

            Assert.NotNull(updated);
            Assert.Equal(update.Brand, updated.Brand);

            Assert.Equal(device.Name, updated.Name);
            Assert.Equal(device.State, updated.State);
            Assert.Equal(device.CreationTime, updated.CreationTime);
            Assert.Equal(device.Id, updated.Id);
        }

        [Fact]
        public async Task UpdateAsync_UpdateState_Ok()
        {
            // Arrange
            var db = TestHelpers.CreateInMemoryDb();
            var service = new DeviceService(db);

            var device = TestHelpers.GetTestDevice();
            db.Devices.Add(device);
            
            await db.SaveChangesAsync();

            var update = TestHelpers.GetEmptyTestDevice(state: DeviceStates.Inactive);

            // Act
            (var result, var updated) = await service.UpdateAsync(device.Id, update);
            
            // Assert
            Assert.Equal(UpdateResult.Updated, result);

            Assert.NotNull(updated);
            Assert.Equal(update.State, updated.State);

            Assert.Equal(device.Name, updated.Name);
            Assert.Equal(device.Brand, updated.Brand);
            Assert.Equal(device.CreationTime, updated.CreationTime);
            Assert.Equal(device.Id, updated.Id);
        }

        [Fact]
        public async Task UpdateAsync_UpdateState_InvalidState()
        {
            // Arrange
            var db = TestHelpers.CreateInMemoryDb();
            var service = new DeviceService(db);

            var device = TestHelpers.GetTestDevice();
            db.Devices.Add(device);
            
            await db.SaveChangesAsync();

            var update = TestHelpers.GetEmptyTestDevice(state: "new_state");

            // Act
            (var result, var updated) = await service.UpdateAsync(device.Id, update);
            
            // Assert
            Assert.Equal(UpdateResult.InvalidState, result);

            Assert.NotNull(updated);
            Assert.NotEqual(update.State, updated.State);

            Assert.Equal(device.Name, updated.Name);
            Assert.Equal(device.Brand, updated.Brand);
            Assert.Equal(device.CreationTime, updated.CreationTime);
            Assert.Equal(device.Id, updated.Id);
        }

        [Fact]
        public async Task UpdateAsync_DeviceNotFound()
        {
            // Arrange
            var db = TestHelpers.CreateInMemoryDb();
            var service = new DeviceService(db);

            var device = TestHelpers.GetTestDevice();
            db.Devices.Add(device);
            
            await db.SaveChangesAsync();

            var update = TestHelpers.GetEmptyTestDevice(name: "new_name");

            // Act
            (var result, var updated) = await service.UpdateAsync(device.Id + 1, update);
            
            // Assert
            Assert.Equal(UpdateResult.NotFound, result);
        }

        [Fact]
        public async Task UpdateAsync_DeviceInUse()
        {
            // Arrange
            var db = TestHelpers.CreateInMemoryDb();
            var service = new DeviceService(db);

            var device = TestHelpers.GetTestDevice(state: DeviceStates.InUse);
            db.Devices.Add(device);
            
            await db.SaveChangesAsync();

            var update = TestHelpers.GetEmptyTestDevice(name: "new_name");

            // Act
            (var result, var updated) = await service.UpdateAsync(device.Id, update);
            
            // Assert
            Assert.Equal(UpdateResult.IsInUse, result);

            Assert.NotNull(updated);
            Assert.NotEqual(update.Name, updated.Name);

            Assert.Equal(device.Name, updated.Name);
            Assert.Equal(device.Brand, updated.Brand);
            Assert.Equal(device.State, updated.State);
            Assert.Equal(device.CreationTime, updated.CreationTime);
            Assert.Equal(device.Id, updated.Id);
        }
    }
}