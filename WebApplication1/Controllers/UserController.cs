using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;

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
        var user = HttpContext.Session.GetString("User");
        var role = HttpContext.Session.GetString("Role");

        if (user == null || role != "User")
            return RedirectToAction("Login", "Auth");

        ViewBag.Contracts = _context.Contracts
            .Include(c => c.Client)
            .Where(c => c.Status == "Active")
            .ToList();

        var requests = _context.ServiceRequests
            .Include(r => r.Contract)
            .ThenInclude(c => c.Client)
            .OrderByDescending(r => r.CreatedDate)
            .ToList();

        return View(requests);
    }

    // =========================
    // LOGOUT
    // =========================
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login", "Auth");
    }
}