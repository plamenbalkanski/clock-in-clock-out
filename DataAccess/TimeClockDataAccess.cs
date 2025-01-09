using Csla;
using TimeClockApi.Models;

namespace TimeClockApi.DataAccess
{
    public interface ITimeClockDataAccess
    {
        Task<TimeClockEntry> ClockIn(int employeeId, string location);
        Task<TimeClockEntry> ClockOut(int employeeId);
        Task<IEnumerable<TimeClockEntry>> GetEmployeeEntries(int employeeId);
    }

    public class TimeClockDataAccess : ITimeClockDataAccess
    {
        private readonly IDataPortal<TimeClockEntry> _portalEntry;
        private readonly IDataPortal<TimeClockEntryList> _portalList;

        public TimeClockDataAccess(
            IDataPortal<TimeClockEntry> portalEntry,
            IDataPortal<TimeClockEntryList> portalList)
        {
            _portalEntry = portalEntry;
            _portalList = portalList;
        }

        public async Task<TimeClockEntry> ClockIn(int employeeId, string location)
        {
            var entry = await _portalEntry.CreateAsync();
            entry.EmployeeId = employeeId;
            entry.ClockInTime = DateTime.UtcNow;
            entry.Location = location;
            await entry.SaveAsync();
            return entry;
        }

        public async Task<TimeClockEntry> ClockOut(int employeeId)
        {
            var entries = await _portalList.FetchAsync(employeeId);
            var activeEntry = entries.FirstOrDefault(e => !e.ClockOutTime.HasValue);
            
            if (activeEntry == null)
                throw new InvalidOperationException("No active clock-in found");

            activeEntry.ClockOutTime = DateTime.UtcNow;
            await activeEntry.SaveAsync();
            return activeEntry;
        }

        public async Task<IEnumerable<TimeClockEntry>> GetEmployeeEntries(int employeeId)
        {
            return await _portalList.FetchAsync(employeeId);
        }
    }
} 