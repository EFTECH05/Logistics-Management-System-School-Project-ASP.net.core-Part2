using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Services;
using System;
using System.Linq;

namespace WebApplication1.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDbContext _context;
        private readonly EmailService _emailService;

        public AuthController(AppDbContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        // ================= LOGIN PAGE =================
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // ================= LOGIN POST =================
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            email = email?.Trim().ToLower();
            password = password?.Trim();

            var user = _context.Users
                .FirstOrDefault(x =>
                    x.Email.ToLower() == email &&
                    x.Password == password);

            if (user == null)
            {
                ViewBag.Error = "Invalid email or password";
                return View();
            }

            // ADMIN → NO OTP
            if (user.Role == "Admin")
            {
                HttpContext.Session.SetString("User", user.Email);
                HttpContext.Session.SetString("Role", user.Role);
                HttpContext.Session.SetInt32("UserId", user.Id);

                return RedirectToAction("Index", "Admin");
            }

            // USER → OTP
            GenerateOtp(user.Email);
            return RedirectToAction("VerifyOtp");
        }

        // ================= REGISTER PAGE =================
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // ================= REGISTER POST =================
        [HttpPost]
        public IActionResult Register(User user)
        {
            if (!ModelState.IsValid)
                return View(user);

            var exists = _context.Users.Any(x =>
                x.Email.ToLower() == user.Email.ToLower() ||
                x.Username.ToLower() == user.Username.ToLower());

            if (exists)
            {
                ViewBag.Error = "User already exists";
                return View(user);
            }

            user.Email = user.Email.Trim().ToLower();
            user.Password = user.Password.Trim();
            user.Role = "User";

            _context.Users.Add(user);
            _context.SaveChanges();

            GenerateOtp(user.Email);

            return RedirectToAction("VerifyOtp");
        }

        // ================= OTP PAGE =================
        [HttpGet]
        public IActionResult VerifyOtp()
        {
            return View();
        }

        // ================= OTP VERIFY =================
        [HttpPost]
        public IActionResult VerifyOtp(string otp)
        {
            var sessionOtp = HttpContext.Session.GetString("OTP");
            var email = HttpContext.Session.GetString("OTP_Email");
            var timeString = HttpContext.Session.GetString("OTP_Time");

            if (sessionOtp == null || email == null || timeString == null)
            {
                ViewBag.Error = "Session expired";
                return View();
            }

            var time = DateTime.Parse(timeString);

            if (DateTime.Now > time.AddMinutes(5))
            {
                ViewBag.Error = "OTP expired";
                return View();
            }

            if (otp != sessionOtp)
            {
                ViewBag.Error = "Invalid OTP";
                return View();
            }

            var user = _context.Users.FirstOrDefault(x => x.Email == email);

            if (user == null)
            {
                ViewBag.Error = "User not found";
                return View();
            }

            HttpContext.Session.SetString("User", user.Email);
            HttpContext.Session.SetString("Role", user.Role);
            HttpContext.Session.SetInt32("UserId", user.Id);

            HttpContext.Session.Remove("OTP");
            HttpContext.Session.Remove("OTP_Email");
            HttpContext.Session.Remove("OTP_Time");

            return RedirectToAction("Index", "User");
        }

        // ================= LOGOUT =================
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        // ================= OTP METHOD =================
        private void GenerateOtp(string email)
        {
            var otp = new Random().Next(100000, 999999).ToString();

            HttpContext.Session.SetString("OTP", otp);
            HttpContext.Session.SetString("OTP_Email", email);
            HttpContext.Session.SetString("OTP_Time", DateTime.Now.ToString());

            _emailService.SendOtp(email, otp);
        }
    }
}