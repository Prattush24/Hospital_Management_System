using hospital.Models.Appointment;
using hospital.Models.Appointment.AppointmentDto;
using hospital.Repository.Interface;
using hospital.Services.Interface;

namespace hospital.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _repo;

        public AppointmentService(IAppointmentRepository repo)
        {
            _repo = repo;
        }

        public async Task BookAppointmentAsync(BookAppointmentDto dto)
        {
            if (dto.PatientId <= 0)
                throw new Exception("Invalid Patient Id.");

            if (dto.DoctorId <= 0)
                throw new Exception("Invalid Doctor Id.");

            if (dto.AppointmentDate <= DateTime.Now)
                throw new Exception("Appointment date must be in the future.");

            await _repo.BookAppointmentAsync(dto);
        }

        public async Task CancelAppointmentAsync(int appointmentId)
        {
            if (appointmentId <= 0)
                throw new Exception("Invalid Appointment Id.");

            await _repo.CancelAppointmentAsync(appointmentId);
        }

        public async Task<List<Appointment>> GetUpcomingAppointmentsAsync()
        {
            return await _repo.GetUpcomingAppointmentsAsync();
        }

        public async Task<List<Appointment>> GetDoctorAppointmentsAsync(int doctorId)
        {
            if (doctorId <= 0)
                throw new Exception("Invalid Doctor Id.");

            return await _repo.GetDoctorAppointmentsAsync(doctorId);
        }

        public async Task<List<Appointment>> GetPatientAppointmentsAsync(int patientId)
        {
            if (patientId <= 0)
                throw new Exception("Invalid Patient Id.");

            return await _repo.GetPatientAppointmentsAsync(patientId);
        }
    }
}
