namespace hospital.Models.Report.ReportDto
{
    public class DoctorAppointmentCountDto
    {
        public string DoctorId { get; set; } = string.Empty;

        public string DoctorName { get; set; } = string.Empty;

        public int TotalAppointments { get; set; }
    }
}
