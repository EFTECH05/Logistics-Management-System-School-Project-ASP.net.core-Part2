using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class AuthController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AuthController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // ================= LOGIN PAGE =================
        public IActionResult Login()
        {
            return View();
        }

        // ================= LOGIN =================
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var client = _httpClientFactory.CreateClient("ApiClient");

            var response = await client.PostAsJsonAsync("api/auth/login", new
            {
                Email = email,
                Password = password
            });

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Invalid email or password";
                return View();
            }

            var result = await response.Content.ReadFromJsonAsync<LoginResponseDto>();

            if (result == null || string.IsNullOrEmpty(result.Token))
            {
                ViewBag.Error = "Server error";
                return View();
            }

            // STORE SESSION
            HttpContext.Session.SetString("Token", result.Token);
            HttpContext.Session.SetString("User", result.Email);
            HttpContext.Session.SetString("Role", result.Role);

            return RedirectToAction("Index", "Admin");
        }

        // ================= REGISTER PAGE =================
        public IActionResult Register()
        {
            return View();
        }

        // ================= REGISTER =================
        [HttpPost]
        public async Task<IActionResult> Register(User model)
        {
            var client = _httpClientFactory.CreateClient("ApiClient");

            var response = await client.PostAsJsonAsync("api/auth/register", model);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = await response.Content.ReadAsStringAsync();
                return View(model);
            }

            TempData["Success"] = "Account created successfully!";
            return RedirectToAction("Login");
        }

        // ================= LOGOUT =================
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }

    // ================= DTO =================
    public class LoginResponseDto
    {
        public string Token { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}