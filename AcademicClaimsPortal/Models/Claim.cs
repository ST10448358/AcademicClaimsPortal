using System.ComponentModel.DataAnnotations;

namespace AcademicClaimsPortal.Models
{
    public class Claim
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Lecturer Name")]
        public string LecturerName { get; set; } = string.Empty;

        [Required]
        public string Department { get; set; } = string.Empty;

        [Required]
        public string Month { get; set; } = string.Empty;

        [Required]
        [Range(2024, 2030)]
        public int Year { get; set; } = DateTime.Now.Year;

        [Required]
        [Display(Name = "Hours Worked")]
        [Range(1, 200)]
        public double HoursWorked { get; set; }

        [Required]
        [Display(Name = "Hourly Rate")]
        [Range(50, 500)]
        public decimal HourlyRate { get; set; }

        [Display(Name = "Total Amount")]
        public decimal TotalAmount => (decimal)HoursWorked * HourlyRate;

        public string Status { get; set; } = "Pending";

        [Display(Name = "Claim Reference")]
        public string ClaimReference { get; set; } = string.Empty;

        [Display(Name = "Submission Date")]
        public DateTime SubmissionDate { get; set; } = DateTime.Now;

        // NEW PROPERTIES FOR COORDINATOR REVIEW
        [Display(Name = "Reviewed By")]
        public string ReviewedBy { get; set; } = string.Empty;

        [Display(Name = "Review Date")]
        public DateTime? ReviewDate { get; set; }
    }
}