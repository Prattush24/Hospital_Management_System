using hospital.Models.Appointment.AppointmentDto;
using hospital.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace hospital.Controllers
{
    // Defines the base route pattern for all actions in this controller
    [Route("api/[controller]/[Action]")]

    // Indicates that this class is an API controller
    [ApiController]
    public class AppointmentController : ControllerBase
    {
       // Service dependency used for appointmentrelated operations
        private readonly IAppointmentService _appointmentService;

        // Constructor for dependency injection
        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        // Book Appointment
        [HttpPost]
        public async Task<IActionResult> BookAppointment([FromBody] BookAppointmentDto dto)
        {
            int appointmentId =
                await _appointmentService.BookAppointmentAsync(dto);

            return Ok($"Appointment Booked Successfully. Your Appointment ID is: {appointmentId}");
        }

        // Cancel Appointment
        [HttpPatch("{appointmentId}/cancel")]
        public async Task<IActionResult> CancelAppointment(int appointmentId)
        {
            // Calls service layer to cancel the specified appointment
            await _appointmentService.CancelAppointmentAsync(appointmentId);

            // Returns success response
            return Ok("Appointment Cancelled Successfully");
        }

        // mark Appointment as completed
        [HttpPatch("{appointmentId}/Mark_completed")]
        public async Task<IActionResult> MarkAppointmentAsCompletedAsync(int appointmentId)
        {
            // Calls service layer to mark the appointment as completed
            await _appointmentService.MarkAppointmentAsCompletedAsync(appointmentId);

            // Returns success response
            return Ok("Appointment Marked as completed Successfully");
        }

        // Get All Upcoming Appointments
        [HttpGet("upcoming")]
        public async Task<IActionResult> GetUpcomingAppointments()
        {
            // Retrieves all upcoming appointments from the service layer
            var appointments =
                    await _appointmentService.GetUpcomingAppointmentsAsync();

            // Returns the appointment list
            return Ok(appointments);
        }

        // Get Appointments By Doctor
        [HttpGet("doctor/{doctorId}")]
        public async Task<IActionResult> GetDoctorAppointments(int doctorId)
        {
            // Retrieves all appointments for the specified doctor
            var appointments =
                    await _appointmentService.GetDoctorAppointmentsAsync(doctorId);

            // Returns the appointment list
            return Ok(appointments);
        }

        // Get Appointments By Patient
        [HttpGet("patient/{patientId}")]
        public async Task<IActionResult> GetPatientAppointments(int patientId)
        {
            // Retrieves all appointments for the specified patient
            var appointments = await _appointmentService.GetPatientAppointmentsAsync(patientId);

            // Returns the appointment list
            return Ok(appointments);
        }
    }
}