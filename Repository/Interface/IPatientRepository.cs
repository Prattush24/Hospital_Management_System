using hospital.Models.Patient.PatientDto;

namespace hospital.Repository.Interface
{
    public interface IPatientRepository
    {
        //Register Patient
        Task<List<GetAllPatientsDto>> GetAllActivePatientsAsync();
        Task<GetPatientByIdDto> GetPatientByIdAsync(int PatientId);
        Task RegisterPatientAsync(CreatePatientDto patient);
        Task UpdatePatientAsync(int PatientId, UpdatePatientDto upatient);
        Task DeactivatePatientAsync(int PatientId);
    }
}
