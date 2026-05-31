using hospital.Models.Doctor.DoctorDto;
using Microsoft.Data.SqlClient;
using System.Data;

namespace hospital.Repository
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly IConfiguration _configuration;

        public DoctorRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<int> AddDoctorAsync(CreateDoctorDto doctor)
        {
            using SqlConnection connection =
                new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));

            using SqlCommand command =
                new SqlCommand("sp_AddDoctor", connection);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@FullName", doctor.FullName);
            command.Parameters.AddWithValue("@Specialization", doctor.Specialization);
            command.Parameters.AddWithValue("@PhoneNumber", doctor.PhoneNumber);
            command.Parameters.AddWithValue("@ConsultationFee", doctor.ConsultationFee);
            command.Parameters.AddWithValue("@IsAvailable", doctor.IsAvailable);

            await connection.OpenAsync();

            return await command.ExecuteNonQueryAsync();
        }

        public async Task<List<GetDoctorDto>> GetDoctorsBySpecializationAsync(string specialization)
        {
            List<GetDoctorDto> doctors = new();

            using SqlConnection connection =
                new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));

            using SqlCommand command =
                new SqlCommand("sp_GetDoctorsBySpecialization", connection);

            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Specialization", specialization);

            await connection.OpenAsync();

            using SqlDataReader reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                doctors.Add(new GetDoctorDto
                {
                    DoctorId = Convert.ToInt32(reader["DoctorId"]),
                    FullName = reader["FullName"].ToString(),
                    Specialization = reader["Specialization"].ToString(),
                    PhoneNumber = reader["PhoneNumber"].ToString(),
                    ConsultationFee = Convert.ToDecimal(reader["ConsultationFee"]),
                    IsAvailable = Convert.ToBoolean(reader["IsAvailable"])
                });
            }

            return doctors;
        }

        public async Task<List<GetDoctorDto>> GetAvailableDoctorsAsync()
        {
            List<GetDoctorDto> doctors = new();

            using SqlConnection connection =
                new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));

            using SqlCommand command =
                new SqlCommand("sp_GetAvailableDoctors", connection);

            command.CommandType = CommandType.StoredProcedure;

            await connection.OpenAsync();

            using SqlDataReader reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                doctors.Add(new GetDoctorDto
                {
                    DoctorId = Convert.ToInt32(reader["DoctorId"]),
                    FullName = reader["FullName"].ToString(),
                    Specialization = reader["Specialization"].ToString(),
                    PhoneNumber = reader["PhoneNumber"].ToString(),
                    ConsultationFee = Convert.ToDecimal(reader["ConsultationFee"]),
                    IsAvailable = true
                });
            }

            return doctors;
        }
    }
}
