using DeviceApi.Data;
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
    }
}
