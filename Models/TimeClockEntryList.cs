using Csla;
using System;
using System.Linq;
using TimeClockApi.Dal;
using System.Threading.Tasks;

namespace TimeClockApi.Models
{
    [Serializable]
    public class TimeClockEntryList : BusinessListBase<TimeClockEntryList, TimeClockEntry>
    {
        [Fetch]
        private async Task Fetch(int employeeId, [Inject] ITimeClockDal dal)
        {
            var data = dal.GetEmployeeEntries(employeeId);
            foreach (var dto in data)
            {
                var entry = await TimeClockEntry.NewTimeClockEntry();
                entry.LoadFromDto(dto);
                Add(entry);
            }
        }
    }
} 