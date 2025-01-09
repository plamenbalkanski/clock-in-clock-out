using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using TimeClockApi.Controllers;
using TimeClockApi.DataAccess;
using TimeClockApi.Models;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace TimeClockApi.Tests.Controllers
{
    public class TimeClockControllerTests
    {
        private readonly Mock<ITimeClockDataAccess> _mockDataAccess;
        private readonly TimeClockController _controller;

        public TimeClockControllerTests()
        {
            _mockDataAccess = new Mock<ITimeClockDataAccess>();
            _controller = new TimeClockController(_mockDataAccess.Object);
        }

        [Fact]
        public async Task ClockIn_Success_ReturnsOkResult()
        {
            // Arrange
            var mockEntry = new Mock<TimeClockEntry>();
            _mockDataAccess.Setup(d => d.ClockIn(1, "Test Location"))
                .ReturnsAsync(mockEntry.Object);

            // Act
            var result = await _controller.ClockIn(1, "Test Location");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Same(mockEntry.Object, okResult.Value);
        }

        [Fact]
        public async Task ClockIn_Exception_ReturnsBadRequest()
        {
            // Arrange
            _mockDataAccess.Setup(d => d.ClockIn(1, "Test Location"))
                .ThrowsAsync(new Exception("Test error"));

            // Act
            var result = await _controller.ClockIn(1, "Test Location");

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Test error", badRequestResult.Value);
        }

        [Fact]
        public async Task ClockOut_Success_ReturnsOkResult()
        {
            // Arrange
            var mockEntry = new Mock<TimeClockEntry>();
            _mockDataAccess.Setup(d => d.ClockOut(1))
                .ReturnsAsync(mockEntry.Object);

            // Act
            var result = await _controller.ClockOut(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Same(mockEntry.Object, okResult.Value);
        }

        [Fact]
        public async Task ClockOut_NoActiveEntry_ReturnsNotFound()
        {
            // Arrange
            _mockDataAccess.Setup(d => d.ClockOut(1))
                .ThrowsAsync(new InvalidOperationException("No active clock-in found"));

            // Act
            var result = await _controller.ClockOut(1);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("No active clock-in found", notFoundResult.Value);
        }

        [Fact]
        public async Task GetEmployeeEntries_Success_ReturnsOkResult()
        {
            // Arrange
            var mockEntries = new List<TimeClockEntry>();
            _mockDataAccess.Setup(d => d.GetEmployeeEntries(1))
                .ReturnsAsync(mockEntries);

            // Act
            var result = await _controller.GetEmployeeEntries(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Same(mockEntries, okResult.Value);
        }

        [Fact]
        public async Task GetEmployeeEntries_Exception_ReturnsBadRequest()
        {
            // Arrange
            _mockDataAccess.Setup(d => d.GetEmployeeEntries(1))
                .ThrowsAsync(new Exception("Test error"));

            // Act
            var result = await _controller.GetEmployeeEntries(1);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Test error", badRequestResult.Value);
        }
    }
} 