using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EgitimPlatformu.Models
{
    public class UserPackage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int PackageId { get; set; }

        [Required]
        public DateTime ActivatedAt { get; set; }

        [Required]
        public DateTime ExpiresAt { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        // Navigation Properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;

        [ForeignKey("PackageId")]
        public virtual Package Package { get; set; } = null!;
    }
} 