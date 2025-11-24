
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

        public async Task<Device?> CreateAsync(Device device)
        {
            if (device.State != null && !IsStateValid(device.State))
            {
                return null;
            }

            var saved = dbContext.Devices.Add(device);
            await dbContext.SaveChangesAsync();
            return saved.Entity;
        }

        public async Task<(UpdateResult, Device?)> UpdateAsync(int id, Device update)
        {
            var saved = await dbContext.Devices.FindAsync(id);
            if (saved == null)
            {
                return (UpdateResult.NotFound, null);
            }

            if (update.State != null && !IsStateValid(update.State))
            {
                return (UpdateResult.InvalidState, saved);
            }

            if (saved.State == DeviceStates.InUse &&
                (update.Brand != null || update.Name != null))
            {
                return (UpdateResult.IsInUse, saved);
            }

            saved.Name = update.Name ?? saved.Name;
            saved.Brand = update.Brand ?? saved.Brand;
            saved.State = update.State ?? saved.State;
            
            await dbContext.SaveChangesAsync();

            return (UpdateResult.Updated, saved);
        }
        
        public async Task<DeleteResult> DeleteAsync(int id)
        {
            var device = await dbContext.Devices.FindAsync(id);

            if (device is null)
                return DeleteResult.NotFound;

            if (device.State == DeviceStates.InUse)
                return DeleteResult.NotAllowed;

            dbContext.Devices.Remove(device);
            await dbContext.SaveChangesAsync();
            return DeleteResult.Deleted;
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

        // Aux
        private bool IsStateValid(string state)
        {
            return state == DeviceStates.Available 
                || state == DeviceStates.Inactive
                || state == DeviceStates.InUse;
        }
    }
}
