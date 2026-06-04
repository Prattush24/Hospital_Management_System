using hospital.Models.Doctor.DoctorDto;
using Microsoft.Data.SqlClient;
using System.Data;

namespace hospital.Repository
{
    // Repository layer responsible for Doctor-related database operations
    public class DoctorRepository : IDoctorRepository
    {
        // IConfiguration used to access connection strings from appsettings.json
        private readonly IConfiguration _configuration;

        // Constructor Dependency Injection
        public DoctorRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Add a new doctor to the database
        public async Task<int> AddDoctorAsync(CreateDoctorDto doctor)
        {
            // Create SQL connection using connection string
            using SqlConnection connection =
                new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));

            // Create command to execute stored procedure
            using SqlCommand command =
                new SqlCommand("sp_AddDoctor", connection);

            command.CommandType = CommandType.StoredProcedure;

            // Pass doctor details as parameters
            command.Parameters.AddWithValue("@FullName", doctor.FullName);
            command.Parameters.AddWithValue("@Specialization", doctor.Specialization);
            command.Parameters.AddWithValue("@PhoneNumber", doctor.PhoneNumber);
            command.Parameters.AddWithValue("@ConsultationFee", doctor.ConsultationFee);
            command.Parameters.AddWithValue("@IsAvailable", doctor.IsAvailable);

            // Open database connection
            await connection.OpenAsync();

            // Execute insert operation and return affected rows
            return await command.ExecuteNonQueryAsync();
        }

        // Retrieve doctors based on specialization
        public async Task<List<GetDoctorDto>> GetDoctorsBySpecializationAsync(string specialization)
        {
            // List to store doctor records
            List<GetDoctorDto> doctors = new();

            using SqlConnection connection =
                new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));

            using SqlCommand command =
                new SqlCommand("sp_GetDoctorsBySpecialization", connection);

            command.CommandType = CommandType.StoredProcedure;

            // Pass specialization parameter
            command.Parameters.AddWithValue("@Specialization", specialization);

            await connection.OpenAsync();

            using SqlDataReader reader = await command.ExecuteReaderAsync();

            // Read records one by one and map to DTO
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

        // Retrieve all doctors who are currently available
        public async Task<List<GetDoctorDto>> GetAvailableDoctorsAsync()
        {
            // List to store available doctors
            List<GetDoctorDto> doctors = new();

            using SqlConnection connection =
                new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));

            using SqlCommand command =
                new SqlCommand("sp_GetAvailableDoctors", connection);

            command.CommandType = CommandType.StoredProcedure;

            await connection.OpenAsync();

            using SqlDataReader reader = await command.ExecuteReaderAsync();

            // Read records and map them to DTO objects
            while (await reader.ReadAsync())
            {
                doctors.Add(new GetDoctorDto
                {
                    DoctorId = Convert.ToInt32(reader["DoctorId"]),
                    FullName = reader["FullName"].ToString(),
                    Specialization = reader["Specialization"].ToString(),
                    PhoneNumber = reader["PhoneNumber"].ToString(),
                    ConsultationFee = Convert.ToDecimal(reader["ConsultationFee"]),

                    // Since this procedure returns only available doctors
                    IsAvailable = true
                });
            }

            return doctors;
        }
    }
}