using hospital.Models.Patient.PatientDto;

namespace hospital.Services.Interface
{
    public interface IPatientServices
    {
        Task<List<GetAllPatientsDto>> GetAllActivePatientsAsync();
        Task<GetPatientByIdDto> GetPatientByIdAsync(int PatientId);
        Task RegisterPatientAsync(CreatePatientDto patient);
        Task UpdatePatientAsync(int PatientId, UpdatePatientDto upatient);
        Task DeactivatePatientAsync(int PatientId);
    }
}

