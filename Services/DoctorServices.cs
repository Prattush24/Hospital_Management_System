using hospital.Models.Doctor.DoctorDto;
using hospital.Services.Interface;

namespace hospital.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _doctorRepository;

        public DoctorService(IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        public async Task<bool> AddDoctorAsync(CreateDoctorDto doctor)
        {
            var rows = await _doctorRepository.AddDoctorAsync(doctor);

            return rows > 0;
        }

        public async Task<List<GetDoctorDto>> GetDoctorsBySpecializationAsync(string specialization)
        {
            return await _doctorRepository.GetDoctorsBySpecializationAsync(specialization);
        }

        public async Task<List<GetDoctorDto>> GetAvailableDoctorsAsync()
        {
            return await _doctorRepository.GetAvailableDoctorsAsync();
        }
    }
}
