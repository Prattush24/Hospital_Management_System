namespace hospital.Models.Patient
{
    public class Patient
    {

        public int PatientId { get; set; }

        public string? FullName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string? Gender { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Email { get; set; }

        public bool IsActive { get; set; }
    }
}