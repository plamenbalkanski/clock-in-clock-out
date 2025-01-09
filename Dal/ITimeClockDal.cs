namespace TimeClockApi.Dal
{
    public interface ITimeClockDal
    {
        TimeClockEntryDto GetEntry(int id);
        IEnumerable<TimeClockEntryDto> GetEmployeeEntries(int employeeId);
        TimeClockEntryDto Insert(int employeeId, DateTime clockInTime, string location);
        void Update(int id, DateTime clockOutTime);
    }
} 