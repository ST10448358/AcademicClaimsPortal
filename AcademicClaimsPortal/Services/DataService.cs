using AcademicClaimsPortal.Models;

namespace AcademicClaimsPortal.Services
{
    public static class DataService
    {
        // Our in-memory data storage (replaces database)
        public static List<Claim> Claims { get; set; } = new List<Claim>();
        public static List<SupportDocument> SupportDocuments { get; set; } = new List<SupportDocument>();

        // Initialize with sample data
        static DataService()
        {
            // Add sample claims for demonstration
            Claims.Add(new Claim
            {
                Id = 1,
                ClaimReference = "CLM-20241117090000",
                LecturerName = "Dr. Kgosiemang",
                Department = "Computer Science",
                Month = "November",
                Year = 2024,
                HoursWorked = 45,
                HourlyRate = 150,
                Status = "Approved by Coordinator",
                SubmissionDate = DateTime.Now.AddDays(-2),
                ReviewedBy = "Coordinator",
                ReviewDate = DateTime.Now.AddDays(-1)
            });

            Claims.Add(new Claim
            {
                Id = 2,
                ClaimReference = "CLM-20241117100000",
                LecturerName = "Prof. Molokwane",
                Department = "Business Studies",
                Month = "November",
                Year = 2024,
                HoursWorked = 38,
                HourlyRate = 140,
                Status = "Pending",
                SubmissionDate = DateTime.Now.AddDays(-1)
            });

            Claims.Add(new Claim
            {
                Id = 3,
                ClaimReference = "CLM-20241117110000",
                LecturerName = "Dr. Motshwanedi",
                Department = "Engineering",
                Month = "November",
                Year = 2024,
                HoursWorked = 42,
                HourlyRate = 160,
                Status = "Pending",
                SubmissionDate = DateTime.Now
            });
        }
    }
}