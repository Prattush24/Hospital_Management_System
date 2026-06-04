using hospital.Models.Appointment;
using hospital.Models.Appointment.AppointmentDto;
using hospital.Models.Patient;
using hospital.Repository;
using hospital.Repository.Interface;
using hospital.Services.Interface;
using Hospital_Management_System.Models.Appointment.AppointmentDto;
using Hospital_Management_System.Services.Interface;

namespace hospital.Services
{
    // Service layer responsible for business logic related to appointments
    public class AppointmentService : IAppointmentService
    {
        // Repository dependency for database operations
        private readonly IAppointmentRepository _repo;

        // Constructor Dependency Injection
        public AppointmentService(IAppointmentRepository repo)
        {
            _repo = repo;
        }

        // Books a new appointment after validating input data
        public async Task<int> BookAppointmentAsync(BookAppointmentDto dto)
        {
            // Validate Patient Id
            if (dto.PatientId <= 0)
                throw new Exception("Invalid Patient Id.");

            // Validate Doctor Id
            if (dto.DoctorId <= 0)
                throw new Exception("Invalid Doctor Id.");

            // Ensure appointment date is in the future
            if (dto.AppointmentDate <= DateTime.Now)
                throw new Exception("Appointment date must be in the future.");

            // Call repository method to save appointment
            int appointmentId =
            await _repo.BookAppointmentAsync(dto);

            return appointmentId;
        }

        // Cancels an existing appointment
        public async Task CancelAppointmentAsync(int appointmentId)
        {
            // Validate Appointment Id
            if (appointmentId <= 0)
                throw new Exception("Invalid Appointment Id.");

            // Call repository method to cancel appointment
            await _repo.CancelAppointmentAsync(appointmentId);
        }

        // Marks an appointment as completed
        public async Task MarkAppointmentAsCompletedAsync(int appointmentId)
        {
            // Validate Appointment Id
            if (appointmentId <= 0)
                throw new Exception("Invalid Appointment Id.");

            // Call repository method to update appointment status
            await _repo.MarkAppointmentAsCompletedAsync(appointmentId);
        }

        // Returns all upcoming appointments
        public async Task<List<Appointment>> GetUpcomingAppointmentsAsync()
        {
            return await _repo.GetUpcomingAppointmentsAsync();
        }

        // Returns appointments for a specific doctor
        public async Task<List<Appointment>> GetDoctorAppointmentsAsync(int doctorId)
        {
            // Validate Doctor Id
            if (doctorId <= 0)
                throw new Exception("Invalid Doctor Id.");

            return await _repo.GetDoctorAppointmentsAsync(doctorId);
        }

        // Returns appointments for a specific patient
        public async Task<List<Appointment>> GetPatientAppointmentsAsync(int patientId)
        {
            // Validate Patient Id
            if (patientId <= 0)
                throw new Exception("Invalid Patient Id.");

            return await _repo.GetPatientAppointmentsAsync(patientId);
        }
    }
}