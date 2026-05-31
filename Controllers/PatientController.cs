using hospital.Services;
using hospital.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using hospital.Models.Patient.PatientDto;

namespace hospital.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientServices _patientService;

        public PatientController(IPatientServices patientService)
        {
            _patientService = patientService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllActivePatients()
        {
            //try
            //{
                //await _patientService.GetAllActivePatientsAsync();

                return Ok(await _patientService.GetAllActivePatientsAsync());
            //}
            //catch (Exception ex)
            //{
            //    return BadRequest(ex.Message);
            //}

        }

        [HttpGet("{PatientId}")]
        public async Task<IActionResult> GetPatientById(int PatientId)
        {
            //try
            //{
            //await _patientService.GetAllActivePatientsAsync();

            return Ok(await _patientService.GetPatientByIdAsync(PatientId));
            //}
            //catch (Exception ex)
            //{
            //    return BadRequest(ex.Message);
            //}

        }



        [HttpPost]
        public async Task<IActionResult> RegisterPatient([FromBody] CreatePatientDto patient)
        {
            try
            {
                await _patientService.RegisterPatientAsync(patient);

                return Ok("Patient Registered Successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPut("{PatientId}")]
        public async Task<IActionResult> UpdatePatientDetails(int PatientId, UpdatePatientDto upatient)
        {
            try
            {
                await _patientService.UpdatePatientAsync(PatientId, upatient);
                return Ok("Update Successfull!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPatch("{PatientId}/deactivate")]
        public async Task<IActionResult> DeactivatePatient(int PatientId)
        {
            try
            {
                await _patientService.DeactivatePatientAsync(PatientId);
                return Ok("Patient Deactivated Successfull!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

    }
}