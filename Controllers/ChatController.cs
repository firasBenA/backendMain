using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestApi.Models;
using TestApi.Repositories;

namespace TestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatRepository _chatRepository;

        public ChatController(IChatRepository chatRepository)
        {
            _chatRepository = chatRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Chat>>> GetAllChats()
        {
            var chats = await _chatRepository.GetAllChatsAsync();
            return Ok(chats);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Chat>> GetChatById(int id)
        {
            var chat = await _chatRepository.GetChatByIdAsync(id);
            if (chat == null)
            {
                return NotFound();
            }
            return Ok(chat);
        }

        [HttpPost]
        public async Task<ActionResult<Chat>> AddChat(Chat chat)
        {
            await _chatRepository.AddChatAsync(chat);
            return CreatedAtAction(nameof(GetChatById), new { id = chat.Id }, chat);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateChat(int id, Chat chat)
        {
            if (id != chat.Id)
            {
                return BadRequest();
            }

            await _chatRepository.UpdateChatAsync(chat);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChat(int id)
        {
            await _chatRepository.DeleteChatAsync(id);
            return NoContent();
        }
    }
}
