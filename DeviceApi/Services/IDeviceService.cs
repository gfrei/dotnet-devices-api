using DeviceApi.Data;
using DeviceApi.Models;

namespace DeviceApi.Services
{
    public interface IDeviceService
    {
        Task<Device?> GetByIdAsync(int id);
        Task<Device?> CreateAsync(Device device);
        Task<(UpdateResult, Device?)> UpdateAsync(int id, Device update);
        Task<DeleteResult> DeleteAsync(int id);
        Task<IEnumerable<Device>> QueryAsync(string? name, string? brand, string? state);
    }
}
