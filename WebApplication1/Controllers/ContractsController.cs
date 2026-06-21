using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class ContractsController : Controller
    {
        private readonly IHttpClientFactory _factory;

        public ContractsController(IHttpClientFactory factory)
        {
            _factory = factory;
        }

        // =========================
        // INDEX (GET ALL CONTRACTS)
        // =========================
        public async Task<IActionResult> Index()
        {
            var client = _factory.CreateClient("ApiClient");

            try
            {
                var contracts = await client.GetFromJsonAsync<List<Contract>>("api/contracts");
                return View(contracts ?? new List<Contract>());
            }
            catch
            {
                ModelState.AddModelError("", "Error loading contracts from API");
                return View(new List<Contract>());
            }
        }

        // =========================
        // CREATE (GET)
        // =========================
        public async Task<IActionResult> Create()
        {
            var client = _factory.CreateClient("ApiClient");

            try
            {
                var clients = await client.GetFromJsonAsync<List<Client>>("api/clients");
                ViewBag.Clients = clients ?? new List<Client>();
            }
            catch
            {
                ViewBag.Clients = new List<Client>();
            }

            return View();
        }

        // =========================
        // CREATE (POST)
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ContractViewModel model)
        {
            var client = _factory.CreateClient("ApiClient");

            if (!ModelState.IsValid)
            {
                ViewBag.Clients = await client.GetFromJsonAsync<List<Client>>("api/clients");
                return View(model);
            }

            using var form = new MultipartFormDataContent();

            form.Add(new StringContent(model.ClientId.ToString()), "ClientId");
            form.Add(new StringContent(model.StartDate.ToString("yyyy-MM-dd")), "StartDate");
            form.Add(new StringContent(model.EndDate.ToString("yyyy-MM-dd")), "EndDate");
            form.Add(new StringContent(model.Status ?? "Draft"), "Status");
            form.Add(new StringContent(model.ServiceLevel ?? "Standard"), "ServiceLevel");

            if (model.PdfFile != null)
            {
                var stream = model.PdfFile.OpenReadStream();
                var file = new StreamContent(stream);
                file.Headers.ContentType =
                    new MediaTypeHeaderValue("application/pdf");

                form.Add(file, "PdfFile", model.PdfFile.FileName);
            }

            var response = await client.PostAsync("api/contracts", form);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError("", error);

                ViewBag.Clients = await client.GetFromJsonAsync<List<Client>>("api/clients");
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        // =========================
        // PATCH: APPROVE / DECLINE
        // =========================
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            var client = _factory.CreateClient("ApiClient");

            var request = new HttpRequestMessage(
                new HttpMethod("PATCH"),
                $"api/contracts/{id}/status?status={status}"
            );

            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Failed to update contract status";
            }

            return RedirectToAction(nameof(Index));
        }

        // =========================
        // OPTIONAL: ADD JWT TOKEN
        // =========================
        private void AddJwtToken(HttpClient client)
        {
            var token = HttpContext.Session.GetString("JWT");

            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
        }
    }
}