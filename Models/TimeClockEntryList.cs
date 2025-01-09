using Csla;
using System;
using System.Linq;

namespace TimeClockApi.Models
{
    [Serializable]
    public class TimeClockEntryList : BusinessListBase<TimeClockEntryList, TimeClockEntry>
    {
        [Fetch]
        private void Fetch(int employeeId, [Inject] ITimeClockDal dal)
        {
            var data = dal.GetEmployeeEntries(employeeId);
            foreach (var item in data)
            {
                using (LoadListItem(item))
                {
                    Add(item);
                }
            }
        }
    }
} 