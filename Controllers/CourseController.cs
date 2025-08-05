using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using EgitimPlatformu.Data;
using EgitimPlatformu.Models;

namespace EgitimPlatformu.Controllers
{
    [Authorize]
    public class CourseController : Controller
    {
        private readonly AppDbContext _context;

        public CourseController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Lessons(int packageId)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                
                if (userId == 0)
                {
                    return RedirectToAction("Index", "Home");
                }

                // Öğrencinin bu paketi aktif edip etmediğini kontrol et
                var userPackage = await _context.UserPackages
                    .Include(up => up.Package)
                    .FirstOrDefaultAsync(up => up.UserId == userId && up.PackageId == packageId && up.IsActive);

                if (userPackage == null)
                {
                    TempData["ErrorMessage"] = "Bu pakete erişiminiz bulunmuyor.";
                    return RedirectToAction("StudentDashboard", "Home");
                }

                ViewBag.Package = userPackage.Package;
                ViewBag.UserPackage = userPackage;
                ViewBag.UserName = User.Identity?.Name;

                return View();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Bir hata oluştu: " + ex.Message;
                return RedirectToAction("StudentDashboard", "Home");
            }
        }
    }
} 