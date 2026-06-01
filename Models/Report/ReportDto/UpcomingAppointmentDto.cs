namespace hospital.Models.Report.ReportDto
{
    public class UpcomingAppointmentDto
    {
        public string PatientName { get; set; } = string.Empty;

        public string DoctorName { get; set; } = string.Empty;

        public string Specialization { get; set; } = string.Empty;

        public DateTime AppointmentDate { get; set; }

        public string Status { get; set; } = string.Empty;
    }
}