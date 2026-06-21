using System.ComponentModel.DataAnnotations;
using WebApplication1.Shared.Models;

namespace WebApplication1.Models
{
    public class User
    {
        public int Id { get; set; }

        public string FullName { get; set; }
        public string Username { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }

        public string Role { get; set; } = "User";

        public string? TempOtp { get; set; }

        // ✅ ADD THIS (FIX FOR OTP EXPIRY ERROR)
        public DateTime? OtpExpiry { get; set; }
    }  
}