using hospital.Services;
using hospital.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using hospital.Models.Patient.PatientDto;

namespace hospital.Controllers
{
    // Defines the base route for all endpoints in this controller
    [Route("api/[controller]/[Action]")]

    // Indicates that this controller responds to web API requests
    [ApiController]
    public class PatientController : ControllerBase
    {
        // Service object used to perform patient-related operations
        private readonly IPatientServices _patientService;

        // Constructor for dependency injection
        public PatientController(IPatientServices patientService)
        {
            _patientService = patientService;
        }

        // Retrieves all active patients
        [HttpGet]
        public async Task<IActionResult> GetAllActivePatients()
        {
            // Calls service layer to get all active patients
            return Ok(await _patientService.GetAllActivePatientsAsync());
        }

        // Retrieves a patient by PatientId
        [HttpGet("{PatientId}")]
        public async Task<IActionResult> GetPatientById(int PatientId)
        {
            // Calls service layer to fetch patient details
            return Ok(await _patientService.GetPatientByIdAsync(PatientId));
        }

        // Registers a new patient
        [HttpPost]
        public async Task<IActionResult> RegisterPatient([FromBody] CreatePatientDto patient)
        {
            // Calls service layer to create a new patient record
            await _patientService.RegisterPatientAsync(patient);

            // Returns success response
            return Ok("Patient Registered Successfully");
        }

        // Updates patient details using PatientId
        [HttpPut("{PatientId}")]
        public async Task<IActionResult> UpdatePatientDetails(int PatientId, UpdatePatientDto upatient)
        {
            // Calls service layer to update patient information
            await _patientService.UpdatePatientAsync(PatientId, upatient);

            // Returns success response
            return Ok("Update Successfull!");
        }

        // Deactivates a patient account
        [HttpPatch("{PatientId}/deactivate")]
        public async Task<IActionResult> DeactivatePatient(int PatientId)
        {
            // Calls service layer to deactivate the patient
            await _patientService.DeactivatePatientAsync(PatientId);

            // Returns success response
            return Ok("Patient Deactivated Successfull!");
        }
    }
}