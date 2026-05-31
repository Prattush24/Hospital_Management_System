using hospital.Data;
using hospital.Models.Patient;
using hospital.Models.Patient.PatientDto;
using hospital.Repository.Interface;
using Microsoft.AspNetCore.Connections;
using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace hospital.Repository
{
    public class PatientRepository : IPatientRepository
    {
        private readonly SqlConnectionFactory _Factory;

        public PatientRepository(SqlConnectionFactory dbFactory)
        {
            _Factory = dbFactory;
        }

        //Get all active patients
        public async Task<List<GetAllPatientsDto>> GetAllActivePatientsAsync()
        {
            var patients = new List<GetAllPatientsDto>();

            using (SqlConnection con = _Factory.CreateConnection())
            {
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("sp_GetActivePatients", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            patients.Add(new GetAllPatientsDto
                            {
                                PatientId = Convert.ToInt32(reader["PatientId"]),
                                FullName = Convert.ToString(reader["FullName"]),
                                DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]),
                                Age = Convert.ToInt32(reader["Age"]),
                                Gender = Convert.ToString(reader["Gender"]),
                                PhoneNumber = Convert.ToString(reader["PhoneNumber"]),
                                Email = Convert.ToString(reader["Email"]),
                                IsActive = Convert.ToBoolean(reader["IsActive"])
                            });
                        }
                    }
                }
            }

            return patients;
        }

        //Get patient by Id

        public async Task<GetPatientByIdDto> GetPatientByIdAsync(int PatientId)
        {
            using (SqlConnection con = _Factory.CreateConnection())
            {
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("sp_GetPatientById", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@PatientId", PatientId);  

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new GetPatientByIdDto
                            {
                                PatientId = Convert.ToInt32(reader["PatientId"]),
                                FullName = Convert.ToString(reader["FullName"])!,
                                DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]),
                                Gender = Convert.ToString(reader["Gender"])!,
                                PhoneNumber = Convert.ToString(reader["PhoneNumber"])!,
                                Email = reader["Email"] == DBNull.Value
                                            ? null
                                            : reader["Email"].ToString(),
                                IsActive = Convert.ToBoolean(reader["IsActive"])
                            };
                        }
                    }
                }
            }

            return null;
        }


        //Regiter patient
        public async Task RegisterPatientAsync(CreatePatientDto patient)
        {
            using (var con = _Factory.CreateConnection())
            {
                con.Open();
                using(SqlCommand cmd = new SqlCommand("sp_RegisterPatient",con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@FullName", patient.FullName);
                    cmd.Parameters.AddWithValue("@DateOfBirth", patient.DateOfBirth);
                    cmd.Parameters.AddWithValue("@Gender", patient.Gender);
                    cmd.Parameters.AddWithValue("@PhoneNumber", patient.PhoneNumber);
                    cmd.Parameters.AddWithValue("@Email", patient.Email);

                    cmd.ExecuteNonQuery();
                }


            }
                await Task.CompletedTask;
        }

        
        //Update patient details
        public async Task UpdatePatientAsync(int PatientId, UpdatePatientDto upatient)
        {
            using (var con = _Factory.CreateConnection())
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("sp_UpdatePatient", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@PatientId", PatientId);
                    cmd.Parameters.AddWithValue(
                        "@FullName",
                        (object?)upatient.FullName ?? DBNull.Value
                    );

                    cmd.Parameters.AddWithValue(
                        "@DateOfBirth",
                        (object?)upatient.DateOfBirth ?? DBNull.Value
                    );

                    cmd.Parameters.AddWithValue(
                        "@Gender",
                        (object?)upatient.Gender ?? DBNull.Value
                    );

                    cmd.Parameters.AddWithValue(
                        "@PhoneNumber",
                        (object?)upatient.PhoneNumber ?? DBNull.Value
                    );

                    cmd.Parameters.AddWithValue(
                        "@Email",
                        (object?)upatient.Email ?? DBNull.Value
                    );
                    cmd.ExecuteNonQuery();
                }
            }

            await Task.CompletedTask;
        }

        //Deactivate patient details
        public async Task DeactivatePatientAsync(int PatientId)
        {
            using (var con = _Factory.CreateConnection())
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("sp_DeactivatePatient", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@PatientId", PatientId);
                    
                    cmd.ExecuteNonQuery();
                }
            }

            await Task.CompletedTask;
        }

        
    }
}
