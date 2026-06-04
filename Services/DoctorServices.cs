using hospital.Models.Doctor.DoctorDto;
using hospital.Services.Interface;

namespace hospital.Services
{
    // Service layer responsible for doctor-related business operations
    public class DoctorService : IDoctorService
    {
        // Repository dependency for doctor database operations
        private readonly IDoctorRepository _doctorRepository;

        // Constructor Dependency Injection
        public DoctorService(IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        // Adds a new doctor to the system
        public async Task<bool> AddDoctorAsync(CreateDoctorDto doctor)
        {
            // Call repository method to insert doctor record
            var rows = await _doctorRepository.AddDoctorAsync(doctor);

            // Return true if insertion was successful
            return rows > 0;
        }

        // Retrieves doctors based on specialization
        public async Task<List<GetDoctorDto>> GetDoctorsBySpecializationAsync(string specialization)
        {
            // Fetch doctors matching the given specialization
            return await _doctorRepository.GetDoctorsBySpecializationAsync(specialization);
        }

        // Retrieves all doctors who are currently available
        public async Task<List<GetDoctorDto>> GetAvailableDoctorsAsync()
        {
            // Fetch available doctors from repository
            return await _doctorRepository.GetAvailableDoctorsAsync();
        }
    }
}