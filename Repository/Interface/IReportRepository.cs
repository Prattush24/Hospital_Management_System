using hospital.Models.Report.ReportDto;

namespace hospital.Repository.Interface
{
    public interface IReportRepository
    {
        Task<List<AppointmentReportDto>> GetAppointmentReportAsync();

        Task<List<DoctorAppointmentCountDto>> GetDoctorAppointmentCountAsync();

        Task<List<RevenueBySpecializationDto>> GetRevenueBySpecializationAsync();

        Task<List<UpcomingAppointmentDto>> GetUpcomingAppointmentsNext7DaysAsync();
    }
}
