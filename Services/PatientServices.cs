using hospital.Models.Patient;
using hospital.Models.Patient.PatientDto;
using hospital.Repository.Interface;
using hospital.Services.Interface;
using Hospital_Management_System.Services;
using Hospital_Management_System.Services.Interface;

namespace hospital.Services
{
    // Service layer responsible for patient-related business logic
    public class PatientServices : IPatientServices
    {
        // Repository dependency for patient database operations
        private readonly IPatientRepository _patientRepo;
        // EmailService dependenc
        private readonly IEmailService _emailService;

        // Constructor Dependency Injection
        public PatientServices(IPatientRepository patientRepo, IEmailService emailService)
        {
            _patientRepo = patientRepo;
            _emailService = emailService;
        }

        
        // Register a new patient after performing validations
        public async Task RegisterPatientAsync(CreatePatientDto patient)
        {
            // Validate patient name
            if (string.IsNullOrWhiteSpace(patient.FullName))
                throw new Exception("Patient name is required.");

            // Validate Date of Birth
            if (patient.DateOfBirth > DateTime.Today)
                throw new Exception("Invalid Date of Birth.");

            // Validate Gender
            if (patient.Gender != "Male" &&
                patient.Gender != "Female" &&
                patient.Gender != "Other")
                throw new Exception("Invalid Gender.");

            // Validate Phone Number
            if (string.IsNullOrWhiteSpace(patient.PhoneNumber))
            {
                throw new Exception("Phone number is required.");
            }

            // Check phone number length
            if (patient.PhoneNumber.Length != 10)
            {
                throw new Exception("Phone number must be 10 digits.");
            }

            // Call repository method to save patient data
            await _patientRepo.RegisterPatientAsync(patient);

            //
            await _emailService.SendEmailAsync(
                patient.Email,
                "Patient Registration Successful",
                $@"
                <h2>Welcome {patient.FullName}</h2>
                <p>Your registration has been completed successfully.</p>
                <p>Thank you for choosing our Hospital Management System.</p>
                ");
        }

        // Update patient details
        public async Task UpdatePatientAsync(int PatientId, UpdatePatientDto upatient)
        {
            // Call repository method to update patient information
            await _patientRepo.UpdatePatientAsync(PatientId, upatient);
        }

        // deactivate a patient record
        public async Task DeactivatePatientAsync(int PatientId)
        {
            // Call repository method to deactivate patient
            await _patientRepo.DeactivatePatientAsync(PatientId);
        }

        // Retrieve all active patients
        public async Task<List<GetAllPatientsDto>> GetAllActivePatientsAsync()
        {
            // Fetch active patients from repository
            var patients = await _patientRepo.GetAllActivePatientsAsync();

            // Check if any active patients exist
            if (patients.Count == 0)
                throw new Exception("No active patients found.");

            return patients;
        }

        // Retrieve patient details by Id
        public async Task<GetPatientByIdDto> GetPatientByIdAsync(int PatientId)
        {
            // Fetch patient data from repository
            GetPatientByIdDto? patients = await _patientRepo.GetPatientByIdAsync(PatientId);

            return patients;
        }
    }
}