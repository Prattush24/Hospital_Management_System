using hospital.Data;
using hospital.Models.Appointment;
using hospital.Models.Appointment.AppointmentDto;
using hospital.Repository.Interface;
using Microsoft.Data.SqlClient;
using System.Data;

namespace hospital.Repository
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly SqlConnectionFactory _Factory;

        public AppointmentRepository(SqlConnectionFactory dbFactory)
        {
            _Factory = dbFactory;
        }

        // Book Appointment
        public async Task BookAppointmentAsync(BookAppointmentDto dto)
        {
            using (SqlConnection con = _Factory.CreateConnection())
            {
                await con.OpenAsync();

                using (SqlCommand cmd =
                    new SqlCommand("sp_BookAppointment", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@PatientId", dto.PatientId);
                    cmd.Parameters.AddWithValue("@DoctorId", dto.DoctorId);
                    cmd.Parameters.AddWithValue("@AppointmentDate", dto.AppointmentDate);

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        // Cancel Appointment
        public async Task CancelAppointmentAsync(int appointmentId)
        {
            using (SqlConnection con = _Factory.CreateConnection())
            {
                await con.OpenAsync();

                using (SqlCommand cmd =
                    new SqlCommand("sp_CancelAppointment", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@AppointmentId", appointmentId);

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        // Get Upcoming Appointments
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
                        while (await reader.ReadAsync())
                        {
                            appointments.Add(MapAppointment(reader));
                        }
                    }
                }
            }

            return appointments;
        }

        // Get Doctor Appointments
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

        // Get Patient Appointments
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

        private Appointment MapAppointment(SqlDataReader reader)
        {
            return new Appointment
            {
                AppointmentId = Convert.ToInt32(reader["AppointmentId"]),
                PatientId = Convert.ToInt32(reader["PatientId"]),
                DoctorId = Convert.ToInt32(reader["DoctorId"]),
                AppointmentDate = Convert.ToDateTime(reader["AppointmentDate"]),
                //Status = reader["Status"].ToString(),

                CancelledAt = reader["CancelledAt"] == DBNull.Value
                    ? null
                    : Convert.ToDateTime(reader["CancelledAt"])
            };
        }
    }
}