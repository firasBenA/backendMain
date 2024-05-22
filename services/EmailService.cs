using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace TestApi.Helpers
{
    public class EmailService
    {
        private readonly string _smtpServer;
        private readonly int _port;
        private readonly string _fromEmail;
        private readonly string _password;

        public EmailService(IConfiguration configuration)
        {
            _smtpServer = configuration["Email:SmtpServer"];
            _port = int.Parse(configuration["Email:Port"]);
            _fromEmail = configuration["Email:FromEmail"];
            _password = configuration["Email:Password"];
        }

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            var client = new SmtpClient(_smtpServer, _port)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(_fromEmail, _password)
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_fromEmail),
                Subject = subject,
                Body = message,
                IsBodyHtml = true
            };

            mailMessage.To.Add(toEmail);

            await client.SendMailAsync(mailMessage);
        }

        public async Task SendVerificationEmailAsync(string email, string verificationToken)
        {
            // Construct the verification email message
            var subject = "Email Verification";
            var body = $"Please click the following link to verify your email: https://yourwebsite.com/verify?token={verificationToken}";

            // Send the email
            using (var message = new MailMessage("your-email@example.com", email))
            {
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = true;

                using (var smtpClient = new SmtpClient("smtp.example.com"))
                {
                    // Configure SMTP client settings (e.g., credentials, port, SSL)
                    smtpClient.Port = 587;
                    smtpClient.EnableSsl = true;
                    smtpClient.Credentials = new System.Net.NetworkCredential("your-smtp-username", "your-smtp-password");

                    // Send the email asynchronously
                    await smtpClient.SendMailAsync(message);
                }
            }
        }
    }
}
