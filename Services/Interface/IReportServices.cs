using hospital.Models.Report.ReportDto;

namespace hospital.Services.Interface
{
    public interface IReportService
    {
        Task<List<AppointmentReportDto>> GetAppointmentReportAsync();

        Task<List<DoctorAppointmentCountDto>> GetDoctorAppointmentCountAsync();

        Task<List<RevenueBySpecializationDto>> GetRevenueBySpecializationAsync();

        Task<List<UpcomingAppointmentDto>> GetUpcomingAppointmentsNext7DaysAsync();
    }
}
