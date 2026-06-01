namespace hospital.Models.Appointment
{
    public class Appointment
    {
        public int AppointmentId { get; set; }

        public int PatientId { get; set; }

        public int DoctorId { get; set; }

        public DateTime AppointmentDate { get; set; }

        public string AppointmentStatus { get; set; } = string.Empty;

        public DateTime? CancelledAt { get; set; }
    }
}
