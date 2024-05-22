using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TestApi.Models;
using TestApi.Repositories;

namespace TestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatMessageRepository _chatMessageRepository;

        public ChatController(IChatMessageRepository chatMessageRepository)
        {
            _chatMessageRepository = chatMessageRepository;
        }

        /*[HttpGet("messages")]
        public ActionResult<IEnumerable<ChatMessage>> GetMessages(int userId1, int userId2)
        {
            var messages = _chatMessageRepository.GetMessages(userId1, userId2);
            return Ok(messages);
        }*/

        [HttpPost("messages")]
        public ActionResult AddMessage([FromBody] ChatMessage message)
        {
            if (message == null)
            {
                return BadRequest("Message cannot be null");
            }

            _chatMessageRepository.AddMessage(message);
            return Ok(message);
        }

        [HttpGet("{userId1}/{userId2}")]
        public async Task<IActionResult> GetMessages(int userId1, int userId2)
        {
            var messages = await _chatMessageRepository.GetMessagesAsync(userId1, userId2);
            return Ok(messages);
        }

       
    }
}
