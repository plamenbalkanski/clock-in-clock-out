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
                var entry = DataPortal.CreateChild<TimeClockEntry>();
                using (entry.BypassPropertyChecks)
                {
                    entry.Id = dto.Id;
                    entry.EmployeeId = dto.EmployeeId;
                    entry.ClockInTime = dto.ClockInTime;
                    entry.ClockOutTime = dto.ClockOutTime;
                    entry.Location = dto.Location;
                }
                Add(entry);
            }
        }
    }
} 