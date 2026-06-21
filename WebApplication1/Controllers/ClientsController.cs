using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class ClientsController : Controller
    {
        private readonly HttpClient _http;

        public ClientsController(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("ApiClient");
        }

        private bool IsAdmin()
        {
            return HttpContext.Session.GetString("Role") == "Admin";
        }

        public async Task<IActionResult> Index()
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Auth");

            var clients = await _http.GetFromJsonAsync<List<Client>>("api/clients");

            return View(clients);
        }

        public IActionResult Create()
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Auth");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Client client)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Auth");

            await _http.PostAsJsonAsync("api/clients", client);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Auth");

            var client = await _http.GetFromJsonAsync<Client>($"api/clients/{id}");

            return View(client);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Client client)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Auth");

            await _http.PutAsJsonAsync($"api/clients/{client.ClientId}", client);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Auth");

            var client = await _http.GetFromJsonAsync<Client>($"api/clients/{id}");

            return View(client);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Auth");

            await _http.DeleteAsync($"api/clients/{id}");

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Auth");

            var client = await _http.GetFromJsonAsync<Client>($"api/clients/{id}");

            return View(client);
        }
    }
}