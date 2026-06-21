using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class ContractsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ContractsController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // =========================
        // SECURITY
        // =========================
        private bool IsAdmin()
        {
            return HttpContext.Session.GetString("Role") == "Admin";
        }

        // =========================
        // INDEX
        // =========================
        public IActionResult Index()
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Auth");

            var contracts = _context.Contracts
                .Include(c => c.Client)
                .ToList();

            return View(contracts);
        }

        // =========================
        // CREATE GET
        // =========================
        public IActionResult Create()
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Auth");

            ViewBag.Clients = _context.Clients.ToList();
            return View();
        }

        // =========================
        // CREATE POST
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ContractViewModel model)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Auth");

            if (model.EndDate <= model.StartDate)
                ModelState.AddModelError("", "End date must be after start date");

            if (!ModelState.IsValid)
            {
                ViewBag.Clients = _context.Clients.ToList();
                return View(model);
            }

            var contract = new Contract
            {
                ClientId = model.ClientId,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                ServiceLevel = model.ServiceLevel,
                Status = model.Status,
                PdfPath = UploadPdf(model.PdfFile)
            };

            _context.Contracts.Add(contract);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // =========================
        // DETAILS
        // =========================
        public IActionResult Details(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Auth");

            var contract = _context.Contracts
                .Include(c => c.Client)
                .FirstOrDefault(c => c.ContractId == id);

            if (contract == null)
                return NotFound();

            return View(contract);
        }

        // =========================
        // EDIT GET
        // =========================
        public IActionResult Edit(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Auth");

            var contract = _context.Contracts.Find(id);

            if (contract == null)
                return NotFound();

            ViewBag.Clients = _context.Clients.ToList();

            return View(new ContractViewModel
            {
                ClientId = contract.ClientId,
                StartDate = contract.StartDate,
                EndDate = contract.EndDate,
                ServiceLevel = contract.ServiceLevel,
                Status = contract.Status
            });
        }

        // =========================
        // EDIT POST
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, ContractViewModel model)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Auth");

            var contract = _context.Contracts.Find(id);

            if (contract == null)
                return NotFound();

            if (model.EndDate <= model.StartDate)
                ModelState.AddModelError("", "End date must be after start date");

            if (!ModelState.IsValid)
            {
                ViewBag.Clients = _context.Clients.ToList();
                return View(model);
            }

            if (model.PdfFile != null)
            {
                if (!string.IsNullOrEmpty(contract.PdfPath))
                {
                    var oldPath = Path.Combine(_env.WebRootPath, contract.PdfPath.TrimStart('/'));
                    if (System.IO.File.Exists(oldPath))
                        System.IO.File.Delete(oldPath);
                }

                contract.PdfPath = UploadPdf(model.PdfFile);
            }

            contract.ClientId = model.ClientId;
            contract.StartDate = model.StartDate;
            contract.EndDate = model.EndDate;
            contract.ServiceLevel = model.ServiceLevel;
            contract.Status = model.Status;

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // =========================
        // DELETE GET
        // =========================
        public IActionResult Delete(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Auth");

            var contract = _context.Contracts
                .Include(c => c.Client)
                .FirstOrDefault(c => c.ContractId == id);

            if (contract == null)
                return NotFound();

            return View(contract);
        }

        // =========================
        // DELETE POST
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Auth");

            var contract = _context.Contracts.Find(id);

            if (contract == null)
                return NotFound();

            if (contract.Status == "Active")
            {
                TempData["Error"] = "Cannot delete ACTIVE contract!";
                return RedirectToAction("Index");
            }

            if (!string.IsNullOrEmpty(contract.PdfPath))
            {
                var path = Path.Combine(_env.WebRootPath, contract.PdfPath.TrimStart('/'));
                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);
            }

            _context.Contracts.Remove(contract);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // =========================
        // UPLOAD HELPER
        // =========================
        private string UploadPdf(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return null;

            string folder = Path.Combine(_env.WebRootPath, "uploads/contracts");

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            string fullPath = Path.Combine(folder, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return "/uploads/contracts/" + fileName;
        }
    }
}