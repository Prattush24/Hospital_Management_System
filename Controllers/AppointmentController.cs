using hospital.Models.Appointment.AppointmentDto;
using hospital.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace hospital.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        // Book Appointment
        [HttpPost]
        public async Task<IActionResult> BookAppointment(
            [FromBody] BookAppointmentDto dto)
        {
            try
            {
                await _appointmentService.BookAppointmentAsync(dto);

                return Ok("Appointment Booked Successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Cancel Appointment
        [HttpPatch("{appointmentId}/cancel")]
        public async Task<IActionResult> CancelAppointment(int appointmentId)
        {
            try
            {
                await _appointmentService.CancelAppointmentAsync(appointmentId);

                return Ok("Appointment Cancelled Successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Get All Upcoming Appointments
        [HttpGet("upcoming")]
        public async Task<IActionResult> GetUpcomingAppointments()
        {
            try
            {
                var appointments =
                    await _appointmentService.GetUpcomingAppointmentsAsync();

                return Ok(appointments);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Get Appointments By Doctor
        [HttpGet("doctor/{doctorId}")]
        public async Task<IActionResult> GetDoctorAppointments(int doctorId)
        {
            try
            {
                var appointments =
                    await _appointmentService.GetDoctorAppointmentsAsync(doctorId);

                return Ok(appointments);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Get Appointments By Patient
        [HttpGet("patient/{patientId}")]
        public async Task<IActionResult> GetPatientAppointments(int patientId)
        {
            try
            {
                var appointments =
                    await _appointmentService.GetPatientAppointmentsAsync(patientId);

                return Ok(appointments);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}