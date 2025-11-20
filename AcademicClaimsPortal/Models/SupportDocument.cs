using System.ComponentModel.DataAnnotations;

namespace AcademicClaimsPortal.Models
{
    public class SupportDocument
    {
        public int Id { get; set; }

        [Required]
        public string FileName { get; set; } = string.Empty;

        [Required]
        public string OriginalFileName { get; set; } = string.Empty;

        [Required]
        public string FilePath { get; set; } = string.Empty;

        public long FileSize { get; set; }

        [Required]
        public string ContentType { get; set; } = string.Empty;

        public DateTime UploadDate { get; set; } = DateTime.Now;

        [Required]
        public string ClaimReference { get; set; } = string.Empty;

        public string DocumentType { get; set; } = "Supporting Document";

        // Change the property to nullable to resolve CS8618
        public Claim? Claim { get; set; }
    }
}