using hospital.Data;
using hospital.Models.Appointment;
using hospital.Models.Appointment.AppointmentDto;
using hospital.Repository.Interface;
using Microsoft.Data.SqlClient;
using System.Data;

namespace hospital.Repository
{
    // Repository layer responsible for Appointment database operations
    public class AppointmentRepository : IAppointmentRepository
    {
        // Database connection factory dependency
        private readonly SqlConnectionFactory _Factory;

        // Constructor Dependency Injection
        public AppointmentRepository(SqlConnectionFactory dbFactory)
        {
            _Factory = dbFactory;
        }

        // Book a new appointment
        public async Task<int> BookAppointmentAsync(BookAppointmentDto dto)
        {
            using (SqlConnection con = _Factory.CreateConnection())
            {
                // Open database connection
                await con.OpenAsync();

                using (SqlCommand cmd =
                    new SqlCommand("sp_BookAppointment", con))
                {
                    // Execute stored procedure
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Pass required parameters
                    cmd.Parameters.AddWithValue("@PatientId", dto.PatientId);
                    cmd.Parameters.AddWithValue("@DoctorId", dto.DoctorId);
                    cmd.Parameters.AddWithValue("@AppointmentDate", dto.AppointmentDate);

                    // Execute insert operation
                    Object result = await cmd.ExecuteScalarAsync();
                    return Convert.ToInt32(result);
                }
            }
        }

        // Cancel an existing appointment
        public async Task CancelAppointmentAsync(int appointmentId)
        {
            using (SqlConnection con = _Factory.CreateConnection())
            {
                await con.OpenAsync();

                using (SqlCommand cmd =
                    new SqlCommand("sp_CancelAppointment", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Pass appointment Id
                    cmd.Parameters.AddWithValue("@AppointmentId", appointmentId);

                    // Execute update operation
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        // Mark appointment status as completed
        public async Task MarkAppointmentAsCompletedAsync(int appointmentId)
        {
            using (SqlConnection con = _Factory.CreateConnection())
            {
                await con.OpenAsync();

                using (SqlCommand cmd =
                    new SqlCommand("sp_MarkAppointmentAsCompleted", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Pass appointment Id
                    cmd.Parameters.AddWithValue("@AppointmentId", appointmentId);

                    // Execute update operation
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        // Retrieve all upcoming appointments
        public async Task<List<Appointment>> GetUpcomingAppointmentsAsync()
        {
            List<Appointment> appointments = new();

            using (SqlConnection con = _Factory.CreateConnection())
            {
                await con.OpenAsync();

                using (SqlCommand cmd =
                    new SqlCommand("sp_GetUpcomingAppointments", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader =
                        await cmd.ExecuteReaderAsync())
                    {
                        // Read each record and map to Appointment object
                        while (await reader.ReadAsync())
                        {
                            appointments.Add(MapAppointment(reader));
                        }
                    }
                }
            }

            return appointments;
        }

        // Retrieve appointments for a specific doctor
        public async Task<List<Appointment>> GetDoctorAppointmentsAsync(int doctorId)
        {
            List<Appointment> appointments = new();

            using (SqlConnection con = _Factory.CreateConnection())
            {
                await con.OpenAsync();

                using (SqlCommand cmd =
                    new SqlCommand("sp_GetDoctorAppointments", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Pass Doctor Id parameter
                    cmd.Parameters.AddWithValue("@DoctorId", doctorId);

                    using (SqlDataReader reader =
                        await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            appointments.Add(MapAppointment(reader));
                        }
                    }
                }
            }

            return appointments;
        }

        // Retrieve appointments for a specific patient
        public async Task<List<Appointment>> GetPatientAppointmentsAsync(int patientId)
        {
            List<Appointment> appointments = new();

            using (SqlConnection con = _Factory.CreateConnection())
            {
                await con.OpenAsync();

                using (SqlCommand cmd =
                    new SqlCommand("sp_GetPatientAppointments", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Pass Patient Id parameter
                    cmd.Parameters.AddWithValue("@PatientId", patientId);

                    using (SqlDataReader reader =
                        await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            appointments.Add(MapAppointment(reader));
                        }
                    }
                }
            }

            return appointments;
        }

        // Helper method to map database record to Appointment model
        private Appointment MapAppointment(SqlDataReader reader)
        {
            return new Appointment
            {
                AppointmentId = Convert.ToInt32(reader["AppointmentId"]),
                PatientId = Convert.ToInt32(reader["PatientId"]),
                DoctorId = Convert.ToInt32(reader["DoctorId"]),
                AppointmentDate = Convert.ToDateTime(reader["AppointmentDate"]),

                // Status field can be mapped if needed
                // Status = reader["Status"].ToString(),

                // Handle nullable CancelledAt field
                CancelledAt = reader["CancelledAt"] == DBNull.Value
                    ? null
                    : Convert.ToDateTime(reader["CancelledAt"])
            };
        }
    }
}