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
                var entry = new TimeClockEntry();
                LoadProperty(entry.IdProperty, dto.Id);
                LoadProperty(entry.EmployeeIdProperty, dto.EmployeeId);
                LoadProperty(entry.ClockInTimeProperty, dto.ClockInTime);
                LoadProperty(entry.ClockOutTimeProperty, dto.ClockOutTime);
                LoadProperty(entry.LocationProperty, dto.Location);
                Add(entry);
            }
        }
    }
} 