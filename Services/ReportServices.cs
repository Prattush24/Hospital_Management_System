using hospital.Models.Report.ReportDto;
using hospital.Repository.Interface;
using hospital.Services.Interface;

namespace hospital.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;

        public ReportService(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        public async Task<List<AppointmentReportDto>> GetAppointmentReportAsync()
        {
            return await _reportRepository.GetAppointmentReportAsync();
        }

        public async Task<List<DoctorAppointmentCountDto>> GetDoctorAppointmentCountAsync()
        {
            return await _reportRepository.GetDoctorAppointmentCountAsync();
        }

        public async Task<List<RevenueBySpecializationDto>> GetRevenueBySpecializationAsync()
        {
            return await _reportRepository.GetRevenueBySpecializationAsync();
        }

        public async Task<List<UpcomingAppointmentDto>> GetUpcomingAppointmentsNext7DaysAsync()
        {
            return await _reportRepository.GetUpcomingAppointmentsNext7DaysAsync();
        }
    }
}
