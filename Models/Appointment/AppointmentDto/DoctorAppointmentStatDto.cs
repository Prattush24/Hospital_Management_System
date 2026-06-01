namespace hospital.Models.Appointment.AppointmentDto
{
    public class DoctorAppointmentStatDto
    {
        public int DoctorId { get; set; }
        public string DoctorName { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;
        public int AppointmentCount { get; set; }
    }
}
