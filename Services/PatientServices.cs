using hospital.Models.Patient;
using hospital.Models.Patient.PatientDto;
using hospital.Repository.Interface;
using hospital.Services.Interface;

namespace hospital.Services
{
    public class PatientServices : IPatientServices
    {
        private readonly IPatientRepository _patientRepo;

        public PatientServices(IPatientRepository patientRepo)
        {
            _patientRepo = patientRepo;
        }



        //Registering patient
        public async Task RegisterPatientAsync(CreatePatientDto patient)
        {
            if (string.IsNullOrWhiteSpace(patient.FullName))
                throw new Exception("Patient name is required.");

            if (patient.DateOfBirth > DateTime.Today)
                throw new Exception("Invalid Date of Birth.");

            if (patient.Gender != "Male" &&
                patient.Gender != "Female" &&
                patient.Gender != "Other")
                throw new Exception("Invalid Gender.");
            if (string.IsNullOrWhiteSpace(patient.PhoneNumber))
            {
                throw new Exception("Phone number is required.");
            }

            if (patient.PhoneNumber.Length != 10)
            {
                throw new Exception("Phone number must be 10 digits.");
            }

            await _patientRepo.RegisterPatientAsync(patient);
        }

        public async Task UpdatePatientAsync(int PatientId, UpdatePatientDto upatient)
        {
            await _patientRepo.UpdatePatientAsync(PatientId, upatient);
        }

        public async Task DeactivatePatientAsync(int PatientId)
        {
            await _patientRepo.DeactivatePatientAsync(PatientId);
        }

        public async Task<List<GetAllPatientsDto>> GetAllActivePatientsAsync()
        {
            var patients = await _patientRepo.GetAllActivePatientsAsync();

            if (patients.Count == 0)
                throw new Exception("No active patients found.");

            return patients;
        }

        public async Task<GetPatientByIdDto> GetPatientByIdAsync(int PatientId)
        {
            GetPatientByIdDto? patients = await _patientRepo.GetPatientByIdAsync(PatientId);

            return patients;
        }
    }
}