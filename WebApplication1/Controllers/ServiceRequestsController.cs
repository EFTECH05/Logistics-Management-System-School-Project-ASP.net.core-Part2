using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class ServiceRequestsController : Controller
    {
        private readonly HttpClient _http;

        public ServiceRequestsController(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("ApiClient");
        }

        // ================= INDEX =================
        public async Task<IActionResult> Index()
        {
            var response = await _http.GetAsync("api/servicerequests");

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "API Error: " + response.StatusCode;
                return View(new List<ServiceRequest>());
            }

            var data = await response.Content.ReadFromJsonAsync<List<ServiceRequest>>();

            return View(data);
        }

        // ================= CREATE GET =================
        public async Task<IActionResult> Create(bool success = false)
        {
            ViewBag.Contracts =
                await _http.GetFromJsonAsync<List<Contract>>("api/contracts");

            ViewBag.Success = success;
            return View();
        }

        // ================= CREATE POST =================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServiceRequest request)
        {
            var response = await _http.PostAsJsonAsync("api/servicerequests", request);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Create", new { success = true });

            ModelState.AddModelError("", "Failed to create request");
            return View(request);
        }
    }
}