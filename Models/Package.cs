using System.ComponentModel.DataAnnotations;

namespace EgitimPlatformu.Models
{
    public class Package
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty; // LGS, TYT, AYT
        
        [Required]
        [StringLength(200)]
        public string Description { get; set; } = string.Empty;
        
        [StringLength(50)]
        public string Grade { get; set; } = string.Empty; // 8. Sınıf, 11-12. Sınıf
        
        public decimal Price { get; set; } = 0;
        
        public int VideoCount { get; set; } = 0;
        
        public int TestCount { get; set; } = 0;
        
        public int DurationMonths { get; set; } = 12; // Paket süresi (ay)
        
        [StringLength(1000)]
        public string Features { get; set; } = string.Empty; // JSON formatında özellikler
        
        public bool IsActive { get; set; } = true;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        [StringLength(7)]
        public string ColorCode { get; set; } = "#007bff"; // Paket rengi
        
        [StringLength(50)]
        public string IconClass { get; set; } = "fas fa-book"; // Font Awesome icon class
    }
}