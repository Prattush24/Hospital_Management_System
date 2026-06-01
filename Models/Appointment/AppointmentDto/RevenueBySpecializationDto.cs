namespace hospital.Models.Appointment.AppointmentDto
{
    public class RevenueBySpecializationDto
    {
        public string Specialization { get; set; } = string.Empty;
        public int TotalAppointments { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
