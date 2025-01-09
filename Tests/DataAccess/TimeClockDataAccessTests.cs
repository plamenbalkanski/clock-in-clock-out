using Xunit;
using Moq;
using Csla;
using TimeClockApi.DataAccess;
using TimeClockApi.Models;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace TimeClockApi.Tests.DataAccess
{
    public class TimeClockDataAccessTests
    {
        private readonly Mock<IDataPortal<TimeClockEntry>> _mockPortalEntry;
        private readonly Mock<IDataPortal<TimeClockEntryList>> _mockPortalList;
        private readonly TimeClockDataAccess _dataAccess;

        public TimeClockDataAccessTests()
        {
            _mockPortalEntry = new Mock<IDataPortal<TimeClockEntry>>();
            _mockPortalList = new Mock<IDataPortal<TimeClockEntryList>>();
            _dataAccess = new TimeClockDataAccess(_mockPortalEntry.Object, _mockPortalList.Object);
        }

        [Fact]
        public async Task ClockIn_ShouldCreateNewEntry()
        {
            // Arrange
            var mockEntry = new Mock<TimeClockEntry>();
            _mockPortalEntry.Setup(p => p.CreateAsync())
                .ReturnsAsync(mockEntry.Object);

            // Act
            await _dataAccess.ClockIn(1, "Test Location");

            // Assert
            _mockPortalEntry.Verify(p => p.CreateAsync(), Times.Once);
            mockEntry.VerifySet(e => e.EmployeeId = 1);
            mockEntry.VerifySet(e => e.Location = "Test Location");
            mockEntry.Verify(e => e.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task ClockOut_WithActiveEntry_ShouldUpdateEntry()
        {
            // Arrange
            var mockEntry = new Mock<TimeClockEntry>();
            mockEntry.Setup(e => e.ClockOutTime).Returns((DateTime?)null);

            var mockList = new Mock<TimeClockEntryList>();
            mockList.Setup(l => l.FirstOrDefault(It.IsAny<Func<TimeClockEntry, bool>>()))
                .Returns(mockEntry.Object);

            _mockPortalList.Setup(p => p.FetchAsync(1))
                .ReturnsAsync(mockList.Object);

            // Act
            await _dataAccess.ClockOut(1);

            // Assert
            _mockPortalList.Verify(p => p.FetchAsync(1), Times.Once);
            mockEntry.Verify(e => e.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task ClockOut_WithNoActiveEntry_ShouldThrowException()
        {
            // Arrange
            var mockList = new Mock<TimeClockEntryList>();
            mockList.Setup(l => l.FirstOrDefault(It.IsAny<Func<TimeClockEntry, bool>>()))
                .Returns((TimeClockEntry)null);

            _mockPortalList.Setup(p => p.FetchAsync(1))
                .ReturnsAsync(mockList.Object);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _dataAccess.ClockOut(1));
        }

        [Fact]
        public async Task GetEmployeeEntries_ShouldReturnEntries()
        {
            // Arrange
            var mockList = new Mock<TimeClockEntryList>();
            _mockPortalList.Setup(p => p.FetchAsync(1))
                .ReturnsAsync(mockList.Object);

            // Act
            var result = await _dataAccess.GetEmployeeEntries(1);

            // Assert
            Assert.Same(mockList.Object, result);
            _mockPortalList.Verify(p => p.FetchAsync(1), Times.Once);
        }
    }
} 