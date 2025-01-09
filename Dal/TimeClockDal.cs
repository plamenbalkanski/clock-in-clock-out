using System.Data.SqlClient;
using Dapper;

namespace TimeClockApi.Dal
{
    public class TimeClockDal : ITimeClockDal
    {
        private readonly string _connectionString;

        public TimeClockDal(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Default");
        }

        public TimeClockEntryDto GetEntry(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            return conn.QuerySingle<TimeClockEntryDto>(
                "SELECT * FROM EmployeeTimeClock WHERE Id = @Id",
                new { Id = id });
        }

        public IEnumerable<TimeClockEntryDto> GetEmployeeEntries(int employeeId)
        {
            using var conn = new SqlConnection(_connectionString);
            return conn.Query<TimeClockEntryDto>(
                "SELECT * FROM EmployeeTimeClock WHERE EmployeeId = @EmployeeId ORDER BY ClockInTime DESC",
                new { EmployeeId = employeeId });
        }

        public TimeClockEntryDto Insert(int employeeId, DateTime clockInTime, string location)
        {
            using var conn = new SqlConnection(_connectionString);
            var id = conn.ExecuteScalar<int>(
                @"INSERT INTO EmployeeTimeClock (EmployeeId, ClockInTime, Location) 
                  VALUES (@EmployeeId, @ClockInTime, @Location);
                  SELECT SCOPE_IDENTITY()",
                new { EmployeeId = employeeId, ClockInTime = clockInTime, Location = location });

            return GetEntry(id);
        }

        public void Update(int id, DateTime clockOutTime)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Execute(
                "UPDATE EmployeeTimeClock SET ClockOutTime = @ClockOutTime WHERE Id = @Id",
                new { Id = id, ClockOutTime = clockOutTime });
        }
    }
} 