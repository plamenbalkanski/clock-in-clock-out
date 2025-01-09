using Npgsql;
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
            using var conn = new NpgsqlConnection(_connectionString);
            return conn.QuerySingle<TimeClockEntryDto>(
                "SELECT * FROM employee_time_clock WHERE id = @Id",
                new { Id = id });
        }

        public IEnumerable<TimeClockEntryDto> GetEmployeeEntries(int employeeId)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            return conn.Query<TimeClockEntryDto>(
                "SELECT * FROM employee_time_clock WHERE employee_id = @EmployeeId ORDER BY clock_in_time DESC",
                new { EmployeeId = employeeId });
        }

        public TimeClockEntryDto Insert(int employeeId, DateTime clockInTime, string location)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var id = conn.ExecuteScalar<int>(
                @"INSERT INTO employee_time_clock (employee_id, clock_in_time, location) 
                  VALUES (@EmployeeId, @ClockInTime, @Location)
                  RETURNING id",
                new { EmployeeId = employeeId, ClockInTime = clockInTime, Location = location });

            return GetEntry(id);
        }

        public void Update(int id, DateTime clockOutTime)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Execute(
                "UPDATE employee_time_clock SET clock_out_time = @ClockOutTime WHERE id = @Id",
                new { Id = id, ClockOutTime = clockOutTime });
        }
    }
} 