using hospital.Models.Appointment;
using hospital.Models.Appointment.AppointmentDto;

namespace hospital.Repository.Interface
{
    public interface IAppointmentRepository
    {
        Task BookAppointmentAsync(BookAppointmentDto dto);

        Task CancelAppointmentAsync(int appointmentId);

        Task<List<Appointment>> GetUpcomingAppointmentsAsync();

        Task<List<Appointment>> GetDoctorAppointmentsAsync(int doctorCode);

        Task<List<Appointment>> GetPatientAppointmentsAsync(int patientCode);
    }
}
