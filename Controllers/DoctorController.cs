using hospital.Models.Doctor.DoctorDto;
using hospital.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace hospital.Controllers
{
    // Defines the base route pattern for all actions in this controller
    [Route("api/[controller]/[Action]")]

    // Indicates that this controller handles API requests
    [ApiController]
    public class DoctorController : ControllerBase
    {
        // Service dependency used for doctor-related operations
        private readonly IDoctorService _doctorService;

        // Constructor for dependency injection
        public DoctorController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        // Adds a new doctor to the system
        [HttpPost("AddDoctor")]
        public async Task<IActionResult> AddDoctor(CreateDoctorDto doctor)
        {
            // Calls service layer to add doctor
            var result = await _doctorService.AddDoctorAsync(doctor);

            // Returns BadRequest if doctor could not be added
            if (!result)
                return BadRequest("Doctor could not be added.");

            // Returns success response
            return Ok("Doctor added successfully.");
        }

        // Retrieves doctors based on specialization
        [HttpGet("GetDoctorsBySpecialization/{specialization}")]
        public async Task<IActionResult> GetDoctorsBySpecialization(string specialization)
        {
            // Calls service layer to get doctors matching the specialization
            var doctors = await _doctorService.GetDoctorsBySpecializationAsync(specialization);

            // Returns the list of doctors
            return Ok(doctors);
        }

        // Retrieves all currently available doctors
        [HttpGet("GetAvailableDoctors")]
        public async Task<IActionResult> GetAvailableDoctors()
        {
            // Calls service layer to fetch available doctors
            var doctors =
                await _doctorService.GetAvailableDoctorsAsync();

            // Returns the list of available doctors
            return Ok(doctors);
        }
    }
}