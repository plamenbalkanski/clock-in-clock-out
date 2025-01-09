namespace TimeClockApi.Dal
{
    public class TimeClockEntryDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime ClockInTime { get; set; }
        public DateTime? ClockOutTime { get; set; }
        public string Location { get; set; }
    }
} 