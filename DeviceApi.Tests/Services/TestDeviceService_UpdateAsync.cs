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

            var update = TestHelpers.GetTestDevice(name: "new_name");

            // Act
            var result = await service.UpdateAsync(device.Id, update);
            
            // Assert
            var updated = await db.Devices.FindAsync(device.Id);
            Assert.Equal(UpdateResult.Updated, result);

            Assert.NotNull(updated);
            Assert.Equal(update.Name, updated.Name);
        }
    }
}