using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EgitimPlatformu.Models
{
    public class PackageRequest
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int PackageId { get; set; }

        [Required]
        public DateTime RequestedAt { get; set; }

        [Required]
        public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected

        public DateTime? RespondedAt { get; set; }

        public string? ResponseMessage { get; set; }

        // Navigation Properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;

        [ForeignKey("PackageId")]
        public virtual Package Package { get; set; } = null!;
    }
} 