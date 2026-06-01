using hospital.Models.Appointment;
using hospital.Models.Appointment.AppointmentDto;

namespace hospital.Services.Interface
{
    public interface IAppointmentService
    {
        Task BookAppointmentAsync(BookAppointmentDto dto);

        Task CancelAppointmentAsync(int appointmentId);

        Task<List<Appointment>> GetUpcomingAppointmentsAsync();

        Task<List<Appointment>> GetDoctorAppointmentsAsync(int doctorId);

        Task<List<Appointment>> GetPatientAppointmentsAsync(int patientId);
    }
}
