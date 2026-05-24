using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using WebApplication1.Data;
using WebApplication1.Models;

public class UserController : Controller
{
    private readonly AppDbContext _context;

    public UserController(AppDbContext context)
    {
        _context = context;
    }

    // =========================
    // USER DASHBOARD
    // =========================
    public IActionResult Index()
    {
        // SESSION CHECK (LOGIN PROTECTION)
        var user = HttpContext.Session.GetString("User");

        if (user == null)
            return RedirectToAction("Login", "Auth");

        // LOAD ACTIVE CONTRACTS
        ViewBag.Contracts = _context.Contracts
            .Include(c => c.Client)
            .Where(c => c.Status == "Active")
            .ToList();

        // LOAD USER SERVICE REQUESTS
        var requests = _context.ServiceRequests
            .Include(r => r.Contract)
            .ThenInclude(c => c.Client)
            .OrderByDescending(r => r.CreatedDate)
            .ToList();

        return View(requests);
    }

    // =========================
    // LOGOUT ACTION
    // =========================
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear(); // remove all session data
        return RedirectToAction("Login", "Auth");
    }
}