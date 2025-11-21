using DeviceApi.Data;
using DeviceApi.Dtos;
using DeviceApi.Endpoints;
using DeviceApi.Models;
using DeviceApi.Services;
using DeviceApi.Tests.Helpers;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using Xunit;

namespace DeviceApi.Tests.Handlers
{
    public class TestDeviceHandlers
    {
       [Fact]
        public async Task GetDeviceById_Ok()
        {
            // Arrange
            var mockService = new Mock<IDeviceService>();
            var device = GenerateDeviceResult();
            
            mockService
                .Setup(s => s.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(device);

            // Act
            var result = await DeviceEndpoints.GetDeviceById(mockService.Object, device.Id);
            var httpResult = Assert.IsType<Ok<Device>>(result);

            // Assert
            Assert.Equal(device, httpResult.Value);
        }
        
       [Fact]
        public async Task GetDeviceById_NotFound()
        {
            // Arrange
            var mockService = new Mock<IDeviceService>();
            
            mockService
                .Setup(s => s.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Device?) null);

            // Act
            var result = await DeviceEndpoints.GetDeviceById(mockService.Object, 0);

            // Assert
            Assert.IsType<NotFound>(result);
        }

       [Fact]
        public async Task CreateDevice_Ok()
        {
            // Arrange
            var mockService = new Mock<IDeviceService>();

            var dto = GenerateTestDeviceCreateDTO();
            var createdDevice = GenerateDeviceResult();

            mockService
                .Setup(s => s.CreateAsync(It.IsAny<Device>()))
                .ReturnsAsync(createdDevice);

            // Act
            var result = await DeviceEndpoints.CreateDevice(mockService.Object, dto);
            var httpResult = Assert.IsType<Created<Device>>(result);

            // Assert
            Assert.Equal(createdDevice, httpResult.Value);
            Assert.Equal("/devices/1", httpResult.Location);
        }

        //aux
        private Device GenerateDeviceResult()
        {
            return new Device 
            { 
                Id = 1,
                Name = "device_name",
                Brand = "device_brand",
                State = "device_state",
                CreationTime = DateTime.UtcNow
            };
        }

        private DeviceCreateDTO GenerateTestDeviceCreateDTO()
        {
            return new DeviceCreateDTO
            { 
                Name = "device_name",
                Brand = "device_brand",
                State = "device_state",
            };
        }
    }

}