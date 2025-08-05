using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using EgitimPlatformu.Data;
using EgitimPlatformu.Models;
using System.Security.Claims;

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

        [HttpPost]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> ActivatePackage(int packageId)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                
                if (userId == 0)
                {
                    return Json(new { success = false, message = "Kullanıcı bulunamadı" });
                }

                var package = await _context.Packages.FindAsync(packageId);
                
                if (package == null)
                {
                    return Json(new { success = false, message = "Paket bulunamadı." });
                }

                if (!package.IsActive)
                {
                    return Json(new { success = false, message = "Bu paket henüz aktif değil." });
                }

                // Öğrencinin bu paketi zaten aktif edip etmediğini kontrol et
                var existingActivation = await _context.UserPackages
                    .FirstOrDefaultAsync(up => up.UserId == userId && up.PackageId == packageId);

                if (existingActivation != null)
                {
                    return Json(new { success = false, message = "Bu paket zaten aktif." });
                }

                // Paketi öğrenci için aktif et
                var userPackage = new UserPackage
                {
                    UserId = userId,
                    PackageId = packageId,
                    ActivatedAt = DateTime.UtcNow,
                    ExpiresAt = DateTime.UtcNow.AddMonths(package.DurationMonths),
                    IsActive = true
                };

                _context.UserPackages.Add(userPackage);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Paket başarıyla aktif edildi." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> RequestPackage([FromBody] PackageRequestModel request)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                
                if (userId == 0)
                {
                    return Json(new { success = false, message = "Kullanıcı bulunamadı" });
                }

                var package = await _context.Packages.FindAsync(request.PackageId);
                
                if (package == null)
                {
                    return Json(new { success = false, message = "Paket bulunamadı." });
                }

                if (!package.IsActive)
                {
                    return Json(new { success = false, message = "Bu paket henüz aktif değil." });
                }

                // Öğrencinin bu paket için zaten talep verip vermediğini kontrol et
                var existingRequest = await _context.PackageRequests
                    .FirstOrDefaultAsync(pr => pr.UserId == userId && pr.PackageId == request.PackageId);

                if (existingRequest != null)
                {
                    return Json(new { success = false, message = "Bu paket için zaten talep vermişsiniz." });
                }

                // Öğrencinin bu paketi zaten aktif edip etmediğini kontrol et
                var existingActivation = await _context.UserPackages
                    .FirstOrDefaultAsync(up => up.UserId == userId && up.PackageId == request.PackageId);

                if (existingActivation != null)
                {
                    return Json(new { success = false, message = "Bu paket zaten aktif." });
                }

                // Paket talebi oluştur
                var packageRequest = new PackageRequest
                {
                    UserId = userId,
                    PackageId = request.PackageId,
                    RequestedAt = DateTime.UtcNow,
                    Status = "Pending"
                };

                _context.PackageRequests.Add(packageRequest);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Paket talebiniz başarıyla gönderildi." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> GetPendingRequests()
        {
            try
            {
                var requests = await _context.PackageRequests
                    .Where(pr => pr.Status == "Pending")
                    .Include(pr => pr.User)
                    .Include(pr => pr.Package)
                    .OrderByDescending(pr => pr.RequestedAt)
                    .Select(pr => new
                    {
                        id = pr.Id,
                        studentName = pr.User.FirstName + " " + pr.User.LastName,
                        studentEmail = pr.User.Email,
                        packageName = pr.Package.Name,
                        packageGrade = pr.Package.Grade,
                        requestedAt = pr.RequestedAt.ToString("dd.MM.yyyy HH:mm")
                    })
                    .ToListAsync();

                return Json(new { success = true, data = requests });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> RespondToRequest(int requestId, string status, string message = "")
        {
            try
            {
                var request = await _context.PackageRequests
                    .Include(pr => pr.Package)
                    .FirstOrDefaultAsync(pr => pr.Id == requestId);

                if (request == null)
                {
                    return Json(new { success = false, message = "Talep bulunamadı." });
                }

                request.Status = status;
                request.RespondedAt = DateTime.UtcNow;
                request.ResponseMessage = message;

                // Eğer onaylandıysa, öğrenci için paketi aktif et
                if (status == "Approved")
                {
                    var userPackage = new UserPackage
                    {
                        UserId = request.UserId,
                        PackageId = request.PackageId,
                        ActivatedAt = DateTime.UtcNow,
                        ExpiresAt = DateTime.UtcNow.AddMonths(request.Package.DurationMonths),
                        IsActive = true
                    };

                    _context.UserPackages.Add(userPackage);
                }

                await _context.SaveChangesAsync();

                return Json(new { success = true, message = $"Talep başarıyla {status.ToLower()} edildi." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> GetMyRequests()
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                
                if (userId == 0)
                {
                    return Json(new { success = false, message = "Kullanıcı bulunamadı" });
                }

                var requests = await _context.PackageRequests
                    .Where(pr => pr.UserId == userId)
                    .Include(pr => pr.Package)
                    .OrderByDescending(pr => pr.RequestedAt)
                    .Select(pr => new
                    {
                        id = pr.Id,
                        packageName = pr.Package.Name,
                        packageGrade = pr.Package.Grade,
                        status = pr.Status,
                        requestedAt = pr.RequestedAt.ToString("dd.MM.yyyy HH:mm"),
                        respondedAt = pr.RespondedAt.HasValue ? pr.RespondedAt.Value.ToString("dd.MM.yyyy HH:mm") : null,
                        responseMessage = pr.ResponseMessage
                    })
                    .ToListAsync();

                return Json(new { success = true, data = requests });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> CheckPackageAccess(int packageId)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                
                if (userId == 0)
                {
                    return Json(new { success = false, message = "Kullanıcı bulunamadı" });
                }

                // Öğrencinin bu paketi aktif edip etmediğini kontrol et
                var userPackage = await _context.UserPackages
                    .FirstOrDefaultAsync(up => up.UserId == userId && up.PackageId == packageId && up.IsActive);

                if (userPackage == null)
                {
                    return Json(new { success = false, message = "Bu pakete erişiminiz bulunmuyor." });
                }

                return Json(new { success = true, message = "Pakete erişim onaylandı." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> GetStudents(int packageId)
        {
            try
            {
                var students = await _context.UserPackages
                    .Where(up => up.PackageId == packageId && up.IsActive)
                    .Include(up => up.User)
                    .Select(up => new
                    {
                        id = up.User.Id,
                        name = up.User.FirstName + " " + up.User.LastName,
                        email = up.User.Email,
                        activatedAt = up.ActivatedAt.ToString("dd.MM.yyyy HH:mm"),
                        expiresAt = up.ExpiresAt.ToString("dd.MM.yyyy HH:mm")
                    })
                    .ToListAsync();

                return Json(new { success = true, data = students });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> GetMyPackages()
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                
                if (userId == 0)
                {
                    return Json(new { success = false, message = "Kullanıcı bulunamadı" });
                }

                var userPackages = await _context.UserPackages
                    .Where(up => up.UserId == userId && up.IsActive)
                    .Include(up => up.Package)
                    .Select(up => new
                    {
                        id = up.Package.Id,
                        name = up.Package.Name,
                        description = up.Package.Description,
                        grade = up.Package.Grade,
                        price = up.Package.Price,
                        videoCount = up.Package.VideoCount,
                        testCount = up.Package.TestCount,
                        activatedAt = up.ActivatedAt.ToString("dd.MM.yyyy"),
                        expiresAt = up.ExpiresAt.ToString("dd.MM.yyyy"),
                        isExpired = up.ExpiresAt < DateTime.UtcNow
                    })
                    .ToListAsync();

                return Json(new { success = true, data = userPackages });
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

    public class PackageRequestModel
    {
        public int PackageId { get; set; }
    }
}