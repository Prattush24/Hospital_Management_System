namespace hospital.Models.Doctor.DoctorDto
{
        public class GetDoctorDto
        {
            public int DoctorId { get; set; }

            public string? FullName { get; set; }

            public string? Specialization { get; set; }

            public string? PhoneNumber { get; set; }

            public decimal ConsultationFee { get; set; }

            public bool IsAvailable { get; set; }
        }
}
