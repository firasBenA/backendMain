using Microsoft.AspNetCore.Mvc;
using EmailApi.Repositories;
using System;
using System.Threading.Tasks;

namespace EmailApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly IEmailRepository _emailRepository;

        public EmailController(IEmailRepository emailRepository)
        {
            _emailRepository = emailRepository;
        }

        [HttpPost]
        public async Task<IActionResult> SendEmailAsync(Email emailModel)
        {
            try
            {
                await _emailRepository.SendEmailAsync(emailModel.To, emailModel.Subject, emailModel.Body);
                return Ok("Email sent successfully!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error sending email: {ex.Message}");
            }
        }
        
    }


}
