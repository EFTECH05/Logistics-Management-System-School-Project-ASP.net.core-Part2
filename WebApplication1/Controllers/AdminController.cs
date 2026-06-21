using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;

namespace WebApplication1.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
                return RedirectToAction("Login", "Auth");

            ViewBag.TotalUsers = _context.Users.Count();
            ViewBag.TotalClients = _context.Clients.Count();
            ViewBag.TotalContracts = _context.Contracts.Count();
            ViewBag.TotalRequests = _context.ServiceRequests.Count();

            return View();
        }
    }
}