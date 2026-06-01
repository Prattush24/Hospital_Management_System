using hospital.Models.Report.ReportDto;
using hospital.Repository.Interface;
using Microsoft.Data.SqlClient;
using System.Data;

namespace hospital.Repository
{
    public class ReportRepository : IReportRepository
    {
        private readonly IConfiguration _configuration;

        public ReportRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(
                _configuration.GetConnectionString("DefaultConnection"));
        }


        //Get appointment report
        public async Task<List<AppointmentReportDto>> GetAppointmentReportAsync()
        {
            var reports = new List<AppointmentReportDto>();

            using var connection = GetConnection();

            using var command =
                new SqlCommand("sp_GetAppointmentReport", connection);

            command.CommandType = CommandType.StoredProcedure;

            await connection.OpenAsync();

            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                reports.Add(new AppointmentReportDto
                {
                    PatientId = reader["PatientId"].ToString()!,
                    PatientName = reader["PatientName"].ToString()!,
                    DoctorId = reader["DoctorId"].ToString()!,
                    DoctorName = reader["DoctorName"].ToString()!,
                    Specialization = reader["Specialization"].ToString()!,
                    AppointmentDate = Convert.ToDateTime(reader["AppointmentDate"]),
                    Status = reader["Status"].ToString()!,
                    ConsultationFee = Convert.ToDecimal(reader["ConsultationFee"])
                });
            }

            return reports;
        }

        //Get appointment count per doctor

        public async Task<List<DoctorAppointmentCountDto>> GetDoctorAppointmentCountAsync()
        {
            var doctors = new List<DoctorAppointmentCountDto>();

            using var connection = GetConnection();

            using var command =
                new SqlCommand("sp_GetDoctorAppointmentCount", connection);

            command.CommandType = CommandType.StoredProcedure;

            await connection.OpenAsync();

            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                doctors.Add(new DoctorAppointmentCountDto
                {
                    DoctorId = reader["DoctorId"].ToString()!,
                    DoctorName = reader["FullName"].ToString()!,
                    TotalAppointments = Convert.ToInt32(reader["TotalAppointments"])
                });
            }

            return doctors;
        }

        //Get revenue by specialization

        public async Task<List<RevenueBySpecializationDto>> GetRevenueBySpecializationAsync()
        {
            var revenues = new List<RevenueBySpecializationDto>();

            using var connection = GetConnection();

            using var command =
                new SqlCommand("sp_GetRevenueBySpecialization", connection);

            command.CommandType = CommandType.StoredProcedure;

            await connection.OpenAsync();

            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                revenues.Add(new RevenueBySpecializationDto
                {
                    Specialization = reader["Specialization"].ToString()!,
                    TotalRevenue = Convert.ToDecimal(reader["TotalRevenue"])
                });
            }

            return revenues;
        }

        //Get all upcoming appointments in next 7 days

        public async Task<List<UpcomingAppointmentDto>> GetUpcomingAppointmentsNext7DaysAsync()
        {
            var appointments = new List<UpcomingAppointmentDto>();

            using var connection = GetConnection();

            using var command =
                new SqlCommand("sp_GetUpcomingAppointmentsNext7Days", connection);

            command.CommandType = CommandType.StoredProcedure;

            await connection.OpenAsync();

            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                appointments.Add(new UpcomingAppointmentDto
                {
                    PatientName = reader["PatientName"].ToString()!,
                    DoctorName = reader["DoctorName"].ToString()!,
                    Specialization = reader["Specialization"].ToString()!,
                    AppointmentDate = Convert.ToDateTime(reader["AppointmentDate"]),
                    Status = reader["Status"].ToString()!
                });
            }

            return appointments;
        }
    }

}