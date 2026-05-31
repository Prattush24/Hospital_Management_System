using System.ComponentModel.DataAnnotations;

namespace hospital.Models.Doctor.DoctorDto
{
    public class CreateDoctorDto
    {
        [Required]
        [StringLength(100)]
        public string? FullName { get; set; }

        [Required]
        [StringLength(100)]
        public string? Specialization { get; set; }

        [Required]
        [Phone]
        [StringLength(15)]
        public string? PhoneNumber { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Consultation fee must be greater than or equal to 0.")]
        public decimal ConsultationFee { get; set; }

        public bool IsAvailable { get; set; } = true;
    }
}