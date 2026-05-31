using hospital.Models.Doctor.DoctorDto;
using hospital.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace hospital.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorService _doctorService;

        public DoctorController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        [HttpPost("AddDoctor")]
        public async Task<IActionResult> AddDoctor(CreateDoctorDto doctor)
        {
            var result = await _doctorService.AddDoctorAsync(doctor);

            if (!result)
                return BadRequest("Doctor could not be added.");

            return Ok("Doctor added successfully.");
        }

        [HttpGet("GetDoctorsBySpecialization/{specialization}")]
        public async Task<IActionResult> GetDoctorsBySpecialization(string specialization)
        {
            var doctors =
                await _doctorService.GetDoctorsBySpecializationAsync(specialization);

            return Ok(doctors);
        }

        [HttpGet("GetAvailableDoctors")]
        public async Task<IActionResult> GetAvailableDoctors()
        {
            var doctors =
                await _doctorService.GetAvailableDoctorsAsync();

            return Ok(doctors);
        }
    }
}
