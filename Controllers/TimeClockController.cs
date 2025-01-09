using Microsoft.AspNetCore.Mvc;
using TimeClockApi.DataAccess;
using TimeClockApi.Models;

namespace TimeClockApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TimeClockController : ControllerBase
    {
        private readonly ITimeClockDataAccess _dataAccess;

        public TimeClockController(ITimeClockDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        [HttpPost("clockin")]
        public async Task<ActionResult<TimeClockEntry>> ClockIn(int employeeId, string location)
        {
            try
            {
                var entry = await _dataAccess.ClockIn(employeeId, location);
                return Ok(entry);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("clockout")]
        public async Task<ActionResult<TimeClockEntry>> ClockOut(int employeeId)
        {
            try
            {
                var entry = await _dataAccess.ClockOut(employeeId);
                return Ok(entry);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("employee/{employeeId}")]
        public async Task<ActionResult<IEnumerable<TimeClockEntry>>> GetEmployeeEntries(int employeeId)
        {
            try
            {
                var entries = await _dataAccess.GetEmployeeEntries(employeeId);
                return Ok(entries);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
} 