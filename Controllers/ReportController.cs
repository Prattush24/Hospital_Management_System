using hospital.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace hospital.Controllers
{
    // Defines the route pattern for all actions in this controller
    [Route("api/[controller]/[Action]")]

    // Marks this class as an API controller and enables API-specific behaviors
    [ApiController]
    public class ReportController : ControllerBase
    {
        // Service dependency used to fetch report data
        private readonly IReportService _reportService;

        // Constructor injection of the report service
        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        // Retrieves complete appointment report
        [HttpGet("appointments")]
        public async Task<IActionResult> GetAppointmentReport()
        {
            // Fetch appointment report data from service layer
            var result = await _reportService.GetAppointmentReportAsync();

            // Return 404 if no records are found
            if (!result.Any())
            {
                return NotFound("No appointment records found.");
            }

            // Return report data with 200 OK status
            return Ok(result);
        }

        // Retrieves appointment count for each doctor
        [HttpGet("doctor_appointment_count")]
        public async Task<IActionResult> GetDoctorAppointmentCount()
        {
            // Fetch doctor appointment count report
            var result = await _reportService.GetDoctorAppointmentCountAsync();

            // Return 404 if no records are found
            if (!result.Any())
            {
                return NotFound("No doctor appointment records found.");
            }

            // Return report data with 200 OK status
            return Ok(result);
        }

        // Retrieves revenue grouped by doctor specialization
        [HttpGet("revenue_by_specialization")]
        public async Task<IActionResult> GetRevenueBySpecialization()
        {
            // Fetch revenue report by specialization
            var result = await _reportService.GetRevenueBySpecializationAsync();

            // Return 404 if no records are found
            if (!result.Any())
            {
                return NotFound("No revenue records found.");
            }

            // Return report data with 200 OK status
            return Ok(result);
        }

        // Retrieves appointments scheduled for the next 7 days
        [HttpGet("upcoming_next_7_days")]
        public async Task<IActionResult> GetUpcomingAppointmentsNext7Days()
        {
            // Fetch upcoming appointments
            var result = await _reportService.GetUpcomingAppointmentsNext7DaysAsync();

            // Return 404 if no records are found
            if (!result.Any())
            {
                return NotFound("No upcoming appointments found.");
            }

            // Return report data with 200 OK status
            return Ok(result);
        }
    }
}