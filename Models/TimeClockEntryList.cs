using Csla;
using System;
using System.Linq;
using TimeClockApi.Dal;

namespace TimeClockApi.Models
{
    [Serializable]
    public class TimeClockEntryList : BusinessListBase<TimeClockEntryList, TimeClockEntry>
    {
        [Fetch]
        private void Fetch(int employeeId, [Inject] ITimeClockDal dal)
        {
            var data = dal.GetEmployeeEntries(employeeId);
            foreach (var dto in data)
            {
                var entry = Child_Create();
                entry.SetProperty(TimeClockEntry.IdProperty, dto.Id);
                entry.SetProperty(TimeClockEntry.EmployeeIdProperty, dto.EmployeeId);
                entry.SetProperty(TimeClockEntry.ClockInTimeProperty, dto.ClockInTime);
                entry.SetProperty(TimeClockEntry.ClockOutTimeProperty, dto.ClockOutTime);
                entry.SetProperty(TimeClockEntry.LocationProperty, dto.Location);
                Add(entry);
            }
        }

        private TimeClockEntry Child_Create()
        {
            return DataPortal.CreateChild<TimeClockEntry>();
        }
    }
} 