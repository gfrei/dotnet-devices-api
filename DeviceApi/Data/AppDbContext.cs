using DeviceApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DeviceApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

        public DbSet<Device> Devices { get; set; }
    }
}