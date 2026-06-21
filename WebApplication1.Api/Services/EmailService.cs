using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace WebApplication1.Api.Services
{
    public class EmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public void SendOtp(string toEmail, string otp)
        {
            try
            {
                var email = _config["Gmail:Email"];
                var appPassword = _config["Gmail:AppPassword"];

                var message = new MimeMessage();

                message.From.Add(new MailboxAddress("Logistica", email));
                message.To.Add(MailboxAddress.Parse(toEmail));
                message.Subject = "OTP Verification";

                message.Body = new TextPart("plain")
                {
                    Text = $"Your OTP is: {otp}"
                };

                using var client = new SmtpClient();

                client.Timeout = 5000; // 🔥 prevent freezing login

                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);

                client.Authenticate(email, appPassword);

                client.Send(message);

                client.Disconnect(true);
            }
            catch (Exception ex)
            {
                // 🔥 IMPORTANT: DO NOT BREAK LOGIN IF EMAIL FAILS
                Console.WriteLine("Email failed: " + ex.Message);
            }
        }
    }
}