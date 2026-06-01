namespace hospital.Models.Appointment.AppointmentDto
{
    public class BookAppointmentDto
    {
        public int PatientId { get; set; }

        public int DoctorId { get; set; }

        public DateTime AppointmentDate { get; set; }
        public string? PatientName { get; internal set; }
    }
}
