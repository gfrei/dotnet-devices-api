using DeviceApi.Models;

namespace DeviceApi.Services
{
    public interface IDeviceService
    {
        Task<Device?> GetByIdAsync(int id);
        Task<Device> CreateAsync(Device device);
        Task<IEnumerable<Device>> GetAllAsync();
    }
}
