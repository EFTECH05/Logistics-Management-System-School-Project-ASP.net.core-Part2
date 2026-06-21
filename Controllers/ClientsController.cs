using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class ClientsController : Controller
    {
        private readonly AppDbContext _context;

        public ClientsController(AppDbContext context)
        {
            _context = context;
        }

        // =========================
        // SECURITY CHECK
        // =========================
        private bool IsAdmin()
        {
            return HttpContext.Session.GetString("Role") == "Admin";
        }

        // =========================
        // LIST ALL CLIENTS
        // =========================
        public IActionResult Index()
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Auth");

            var clients = _context.Clients.ToList();
            return View(clients);
        }

        // =========================
        // CREATE (GET)
        // =========================
        public IActionResult Create()
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Auth");

            return View();
        }

        // =========================
        // CREATE (POST)
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Client client)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Auth");

            if (ModelState.IsValid)
            {
                _context.Clients.Add(client);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(client);
        }

        // =========================
        // EDIT (GET)
        // =========================
        public IActionResult Edit(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Auth");

            var client = _context.Clients.FirstOrDefault(c => c.ClientId == id);

            if (client == null)
                return NotFound();

            return View(client);
        }

        // =========================
        // EDIT (POST)
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Client client)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Auth");

            if (ModelState.IsValid)
            {
                _context.Clients.Update(client);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(client);
        }

        // =========================
        // DELETE (GET)
        // =========================
        public IActionResult Delete(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Auth");

            var client = _context.Clients.FirstOrDefault(c => c.ClientId == id);

            if (client == null)
                return NotFound();

            return View(client);
        }

        // =========================
        // DELETE CONFIRM
        // =========================
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Auth");

            var client = _context.Clients.Find(id);

            if (client != null)
            {
                _context.Clients.Remove(client);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }

        // =========================
        // DETAILS (OPTIONAL)
        // =========================
        public IActionResult Details(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Auth");

            var client = _context.Clients.FirstOrDefault(c => c.ClientId == id);

            if (client == null)
                return NotFound();

            return View(client);
        }
    }
}