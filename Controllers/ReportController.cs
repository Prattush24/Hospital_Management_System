using hospital.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace hospital.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("appointments")]
        public async Task<IActionResult> GetAppointmentReport()
        {
            var result = await _reportService.GetAppointmentReportAsync();

            if (!result.Any())
            {
                return NotFound("No appointment records found.");
            }
                

            return Ok(result);
            
        }

        [HttpGet("doctor_appointment_count")]
        public async Task<IActionResult> GetDoctorAppointmentCount()
        {
            var result = await _reportService.GetDoctorAppointmentCountAsync();

            if (!result.Any())
            {
                return NotFound("No doctor appointment records found.");
            }
                    

            return Ok(result);
            
        }

        [HttpGet("revenue_by_specialization")]
        public async Task<IActionResult> GetRevenueBySpecialization()
        {
            var result = await _reportService.GetRevenueBySpecializationAsync();

            if (!result.Any())
            {
                return NotFound("No revenue records found.");
            }
                    

            return Ok(result);
           
        }


        [HttpGet("upcoming_next_7_days")]
        public async Task<IActionResult> GetUpcomingAppointmentsNext7Days()
        {
            var result = await _reportService.GetUpcomingAppointmentsNext7DaysAsync();

            if (!result.Any())
            {
                return NotFound("No upcoming appointments found.");
            }

            return Ok(result);

        }
            
    }
}
