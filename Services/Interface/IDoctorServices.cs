using hospital.Models.Doctor.DoctorDto;

namespace hospital.Services.Interface
{
    public interface IDoctorService
    {
        Task<bool> AddDoctorAsync(CreateDoctorDto doctor);

        Task<List<GetDoctorDto>> GetDoctorsBySpecializationAsync(string specialization);

        Task<List<GetDoctorDto>> GetAvailableDoctorsAsync();
    }
}
