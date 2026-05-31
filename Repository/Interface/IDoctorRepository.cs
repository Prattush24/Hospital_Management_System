using hospital.Models.Doctor.DoctorDto;

public interface IDoctorRepository
{
    Task<int> AddDoctorAsync(CreateDoctorDto doctor);

    Task<List<GetDoctorDto>> GetDoctorsBySpecializationAsync(string specialization);

    Task<List<GetDoctorDto>> GetAvailableDoctorsAsync();
}
