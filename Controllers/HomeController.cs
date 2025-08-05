using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EgitimPlatformu.Models;
using EgitimPlatformu.Data;
using Microsoft.EntityFrameworkCore;

namespace EgitimPlatformu.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly AppDbContext _context;

    public HomeController(ILogger<HomeController> logger, AppDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> TeacherDashboard()
    {
        ViewBag.UserName = User.Identity?.Name;
        
        // Gerçek öğrenci sayısını al
        var studentCount = await _context.Users
            .Where(u => u.Role == "Student" && u.IsActive)
            .CountAsync();
        
        ViewBag.StudentCount = studentCount;
        
        return View();
    }

    [Authorize(Roles = "Student")]
    public IActionResult StudentDashboard()
    {
        ViewBag.UserName = User.Identity?.Name;
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
