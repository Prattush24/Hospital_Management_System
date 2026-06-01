namespace hospital.Models.Appointment.AppointmentDto
{
    public class AppointmentReportDto
    {
        public int AppointmentId { get; set; }
        public string? PatientName { get; set; }
        public string? DoctorName { get; set; }
        public string? Specialization { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public string? AppointmentStatus { get; set; }
        public decimal ConsultationFee { get; set; }
    }
}