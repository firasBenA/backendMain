using System.Threading.Tasks;

namespace EmailApi.Repositories
{
    public interface IEmailRepository
    {
        Task SendEmailAsync(string to, string subject, string body);
        Task SendVerificationEmailAsync(string to, string verificationCode); // Add this method


    }
}
