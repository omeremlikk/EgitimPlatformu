using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using EgitimPlatformu.Data;
using EgitimPlatformu.Models;

namespace EgitimPlatformu.Controllers
{
    [Authorize]
    public class PackageController : Controller
    {
        private readonly AppDbContext _context;

        public PackageController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var packages = await _context.Packages
                    .OrderBy(p => p.Id)
                    .ToListAsync();

                return Json(new { success = true, data = packages });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var package = await _context.Packages.FindAsync(id);
                
                if (package == null)
                {
                    return Json(new { success = false, message = "Paket bulunamadı." });
                }

                return Json(new { success = true, data = package });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Update([FromBody] Package packageData)
        {
            try
            {
                var existingPackage = await _context.Packages.FindAsync(packageData.Id);
                
                if (existingPackage == null)
                {
                    return Json(new { success = false, message = "Paket bulunamadı." });
                }

                // Güncellenebilir alanları güncelle
                existingPackage.Description = packageData.Description;
                existingPackage.Price = packageData.Price;
                existingPackage.VideoCount = packageData.VideoCount;
                existingPackage.TestCount = packageData.TestCount;
                existingPackage.DurationMonths = packageData.DurationMonths;
                existingPackage.Features = packageData.Features;
                existingPackage.IsActive = packageData.IsActive;
                existingPackage.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Paket başarıyla güncellendi." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }

    public class PackageUpdateRequest
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int VideoCount { get; set; }
        public int TestCount { get; set; }
        public int DurationMonths { get; set; }
        public string Features { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}