using hospital.Models.Appointment;
using hospital.Models.Appointment.AppointmentDto;
using Hospital_Management_System.Models.Appointment.AppointmentDto;

namespace hospital.Services.Interface
{
    public interface IAppointmentService
    {
        Task<int> BookAppointmentAsync(BookAppointmentDto dto);

        Task CancelAppointmentAsync(int appointmentId);

        Task MarkAppointmentAsCompletedAsync(int appointmentId);

        Task<List<Appointment>> GetUpcomingAppointmentsAsync();

        Task<List<Appointment>> GetDoctorAppointmentsAsync(int doctorId);

        Task<List<Appointment>> GetPatientAppointmentsAsync(int patientId);
    }
}
