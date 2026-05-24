using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Utils;
using System.IO;

namespace WebApplication1.Services
{
    public class EmailService
    {
        // Reads values from appsettings.json (Gmail credentials)
        private readonly IConfiguration _config;

        // Gives access to wwwroot folder (for images like logo)
        private readonly IWebHostEnvironment _env;

        // Constructor injects configuration + environment
        public EmailService(IConfiguration config, IWebHostEnvironment env)
        {
            _config = config;
            _env = env;
        }

        // =========================================================
        // SEND OTP EMAIL FUNCTION
        // =========================================================
        public void SendOtp(string toEmail, string otp)
        {
            // 1. Get Gmail credentials from appsettings.json
            var email = _config["Gmail:Email"];
            var appPassword = _config["Gmail:AppPassword"];

            // 2. Create email message object
            var message = new MimeMessage();

            // 3. Set sender details (company name + email)
            message.From.Add(new MailboxAddress("Logistica", email));

            // 4. Recipient email (user who receives OTP)
            message.To.Add(MailboxAddress.Parse(toEmail));

            // 5. Email subject line
            message.Subject = "OTP Verification - Our Logistics System";

            // =========================================================
            // 6. LOAD COMPANY LOGO FROM wwwroot/img/logistic.png
            // =========================================================
            var logoPath = Path.Combine(_env.WebRootPath, "img", "logistic.png");

            // BodyBuilder helps build HTML + attachments
            var builder = new BodyBuilder();

            // Add image as embedded resource (NOT attachment)
            var image = builder.LinkedResources.Add(logoPath);

            // Generate unique Content-ID for image reference
            image.ContentId = MimeUtils.GenerateMessageId();

            // =========================================================
            // 7. HTML EMAIL CONTENT (DESIGN + OTP)
            // =========================================================
            builder.HtmlBody = $@"
            <div style='font-family:Segoe UI, sans-serif; background:#0b0b0b; padding:30px; color:white;'>

                <!-- LOGO SECTION -->
                <div style='text-align:center; margin-bottom:20px;'>
                    <img src='cid:{image.ContentId}' style='width:120px;border-radius:10px;' />
                </div>

                <!-- TITLE -->
                <h2 style='color:#ff7a00; text-align:center;'>
                    Logistica OTP Verification
                </h2>

                <!-- MESSAGE -->
                <p style='text-align:center; font-size:16px;'>
                    Your verification code is:
                </p>

                <!-- OTP CODE -->
                <div style='text-align:center; font-size:32px; letter-spacing:6px;
                            color:#ff7a00; font-weight:bold; margin:20px 0;'>
                    {otp}
                </div>

                <!-- EXPIRY NOTE -->
                <p style='text-align:center; font-size:13px; opacity:0.7;'>
                    This code will expire in 5 minutes.
                </p>

                <hr style='border:1px solid #222; margin:20px 0;' />

                <!-- FOOTER -->
                <p style='text-align:center; font-size:12px; opacity:0.6;'>
                    © Logistica  - Secure Delivery Platform
                </p>
            </div>";

            // Convert HTML into email body
            message.Body = builder.ToMessageBody();

            // =========================================================
            // 8. SEND EMAIL USING SMTP (GMAIL)
            // =========================================================
            using var client = new SmtpClient();

            // FIX SSL/TLS CERTIFICATE ISSUE (common Gmail dev issue)
            client.ServerCertificateValidationCallback = (s, c, h, e) => true;

            // Connect to Gmail SMTP server
            client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);

            // Login using Gmail credentials
            client.Authenticate(email, appPassword);

            // Send email
            client.Send(message);

            // Disconnect cleanly
            client.Disconnect(true);
        }
    }
}