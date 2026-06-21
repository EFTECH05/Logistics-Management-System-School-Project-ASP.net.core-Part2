namespace WebApplication1.Shared.DTOs
{
    public class LoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginResponseDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public bool RequiresOtp { get; set; }
        public string Otp { get; set; }
    }

    public class OtpDto
    {
        public string Email { get; set; }
        public string Otp { get; set; }
    }
}
