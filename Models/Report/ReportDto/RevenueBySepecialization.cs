namespace hospital.Models.Report.ReportDto
{
    public class RevenueBySpecializationDto
    {
        public string Specialization { get; set; } = string.Empty;

        public decimal TotalRevenue { get; set; }
    }
}
