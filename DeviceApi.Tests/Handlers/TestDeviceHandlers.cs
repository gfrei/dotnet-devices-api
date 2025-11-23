using DeviceApi.Data;
using DeviceApi.Dtos;
using DeviceApi.Handlers;
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
            var device = TestHelpers.GetTestDevice();
            
            mockService
                .Setup(s => s.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(device);

            // Act
            var result = await DeviceHandlers.GetDeviceById(mockService.Object, device.Id);
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
            var result = await DeviceHandlers.GetDeviceById(mockService.Object, 0);

            // Assert
            Assert.IsType<NotFound>(result);
        }

       [Fact]
        public async Task CreateDevice_Ok()
        {
            // Arrange
            var mockService = new Mock<IDeviceService>();

            var dto = GenerateTestDeviceCreateDTO();
            var createdDevice = TestHelpers.GetTestDevice();

            mockService
                .Setup(s => s.CreateAsync(It.IsAny<Device>()))
                .ReturnsAsync(createdDevice);

            // Act
            var result = await DeviceHandlers.CreateDevice(mockService.Object, dto);
            var httpResult = Assert.IsType<Created<Device>>(result);

            // Assert
            Assert.Equal(createdDevice, httpResult.Value);
            Assert.Equal("/devices/1", httpResult.Location);
        }

       [Fact]
        public async Task QueryDevices_Ok()
        {
            // Arrange
            var expected = new List<Device>{ };
            var mockService = new Mock<IDeviceService>();
            mockService
                .Setup(s => s.QueryAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(expected);

            // Act
            var result = await DeviceHandlers.QueryDevices(mockService.Object, null, null, null);

            // Assert
            var okResult = Assert.IsType<Ok<IEnumerable<Device>>>(result);
            Assert.Equal(expected, okResult.Value);
        }

       [Fact]
        public async Task DeleteDeviceById_NoContent()
        {
            // Arrange
            var mockService = new Mock<IDeviceService>();
            mockService
                .Setup(s => s.DeleteAsync(It.IsAny<int>()));

            // Act
            var result = await DeviceHandlers.DeleteDeviceById(mockService.Object, 1);

            // Assert
            var okResult = Assert.IsType<NoContent>(result);
        }

        //aux
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