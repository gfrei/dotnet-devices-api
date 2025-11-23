using DeviceApi.Data;
using DeviceApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DeviceApi.Tests.Helpers
{
    public static class TestHelpers
    {
        public static AppDbContext CreateInMemoryDb()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        public static Device GetTestDevice(int id = 1, string name = "name", string brand = "brand", string state = DeviceStates.Available)
        {
            return new Device {
                Id = id,
                Name = name,
                Brand = brand,
                State = state,
                CreationTime = DateTime.UtcNow
            };
        }


        public static Device GetEmptyTestDevice(int id = 1, string? name = null, string? brand = null, string? state = null)
        {
            return new Device {
                Id = id,
                Name = name,
                Brand = brand,
                State = state,
                CreationTime = DateTime.UtcNow
            };
        }

        public static void AssertEqualDevices(Device deviceA, Device deviceB)
        {
            Assert.Equal(deviceA.Id, deviceB.Id);
            Assert.Equal(deviceA.Name, deviceB.Name);
            Assert.Equal(deviceA.Brand, deviceB.Brand);
            Assert.Equal(deviceA.State, deviceB.State);
            Assert.Equal(deviceA.CreationTime, deviceB.CreationTime);
        }
    }
}
