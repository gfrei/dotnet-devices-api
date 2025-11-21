
using DeviceApi.Models;
using DeviceApi.Data;
using Microsoft.EntityFrameworkCore;


namespace DeviceApi.Services
{
    public class DeviceService : IDeviceService
    {
        private readonly AppDbContext dbContext;

        public DeviceService(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Device?> GetByIdAsync(int id)
        {
            return await dbContext.Devices.FindAsync(id);
        }

        public async Task<Device> CreateAsync(Device device)
        {
            dbContext.Devices.Add(device);
            await dbContext.SaveChangesAsync();
            return device;
        }
    }
}
