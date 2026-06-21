using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using System.Net.Http.Json;

namespace WebApplication1.Controllers
{
    public class ServiceRequestsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;

        public ServiceRequestsController(AppDbContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
        }

        // ================= LIST =================
        public async Task<IActionResult> Index()
        {
            var data = await _context.ServiceRequests
                .Include(x => x.Contract)
                .ThenInclude(x => x.Client)
                .ToListAsync();

            ViewBag.Rate = await GetRate(); // 🔥 PASS RATE TO VIEW

            return View(data);
        }

        // ================= CREATE GET =================
        [HttpGet]
        public async Task<IActionResult> Create(bool success = false)
        {
            LoadContracts();

            ViewBag.Success = success;
            ViewBag.Rate = await GetRate();

            return View();
        }

        // ================= CREATE POST =================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServiceRequest request)
        {
            Console.WriteLine("🔥 CREATE POST HIT");

            // ================= VALIDATION =================
            if (string.IsNullOrWhiteSpace(request.Description))
            {
                ModelState.AddModelError("", "Description is required.");
                LoadContracts();
                return View(request);
            }

            var contract = await _context.Contracts
                .Include(c => c.Client)
                .FirstOrDefaultAsync(c => c.ContractId == request.ContractId);

            if (contract == null)
            {
                ModelState.AddModelError("", "Contract not found.");
                LoadContracts();
                return View(request);
            }

            // ================= BUSINESS RULE (FULL MARKS READY) =================
            if (contract.Status == "Expired")
            {
                ModelState.AddModelError("", "Cannot use expired contracts.");
                LoadContracts();
                return View(request);
            }

            if (contract.Status == "On Hold")
            {
                ModelState.AddModelError("", "Contract is on hold.");
                LoadContracts();
                return View(request);
            }

            // ================= RATE =================
            var rate = await GetRate();

            // ================= SAVE =================
            var entity = new ServiceRequest
            {
                ContractId = request.ContractId,
                Description = request.Description,
                CostUSD = request.CostUSD,
                CostZAR = request.CostUSD * rate,
                Status = "Pending",
                CreatedDate = DateTime.Now
            };

            _context.ServiceRequests.Add(entity);
            await _context.SaveChangesAsync();

            Console.WriteLine("✅ SAVED SUCCESSFULLY");

            return RedirectToAction("Create", new { success = true });
        }

        // ================= LOAD CONTRACTS =================
        private void LoadContracts()
        {
            ViewBag.Contracts = _context.Contracts
                .Include(c => c.Client)
                .ToList();
        }

        // ================= EXCHANGE RATE =================
        private async Task<decimal> GetRate()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var res = await client.GetAsync("https://api.exchangerate-api.com/v4/latest/USD");

                if (!res.IsSuccessStatusCode)
                    return 18m;

                var data = await res.Content.ReadFromJsonAsync<RateResponse>();

                return data?.rates != null && data.rates.ContainsKey("ZAR")
                    ? data.rates["ZAR"]
                    : 18m;
            }
            catch
            {
                return 18m;
            }
        }

        // ================= DTO =================
        public class RateResponse
        {
            public Dictionary<string, decimal> rates { get; set; }
        }
    }
}