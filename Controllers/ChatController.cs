using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace SignalRIntro.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IChatMessageRepository _chatMessageRepository;

        public ChatController(IChatMessageRepository chatMessageRepository)
        {
            _chatMessageRepository = chatMessageRepository;
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(string message)
        {
            await _chatMessageRepository.SendMessage(message);
            return NoContent();
        }
    }
}
