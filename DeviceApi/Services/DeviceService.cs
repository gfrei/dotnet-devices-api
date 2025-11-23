
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
            return device; // TODO: check this
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var device = await dbContext.Devices.FindAsync(id);

            if (device is null)
                return false;

            dbContext.Devices.Remove(device);
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Device>> QueryAsync(string? name, string? brand, string? state)
        {
            IQueryable<Device> query = dbContext.Devices;

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(p => p.Name == name);
            }

            if (!string.IsNullOrEmpty(brand))
            {
                query = query.Where(p => p.Brand == brand);
            }

            if (!string.IsNullOrEmpty(state))
            {
                query = query.Where(p => p.State == state);
            }

            return await query.ToListAsync();
        }
    }
}
