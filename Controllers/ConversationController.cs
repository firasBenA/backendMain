using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConversationApi.Repositories;

namespace YourNamespace.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConversationController : ControllerBase
    {
        private readonly IConversationRepository _conversationRepository;
        private readonly IMessageRepository _messageRepository;


        public ConversationController(IConversationRepository conversationRepository, IMessageRepository messageRepository)
        {
            _conversationRepository = conversationRepository;
            _messageRepository = messageRepository;
        }

        [HttpGet("{conversationId}")]
        public async Task<ActionResult<Conversation>> GetConversation(int conversationId)
        {
            var conversation = await _conversationRepository.GetConversationById(conversationId);
            if (conversation == null)
            {
                return NotFound();
            }
            return Ok(conversation);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Conversation>>> GetAllConversations()
        {
            var conversations = await _conversationRepository.GetAllConversations();
            return Ok(conversations);
        }

        [HttpPost]
        public async Task<ActionResult<Conversation>> CreateConversation(Conversation conversation)
        {
            var createdConversation = await _conversationRepository.CreateConversation(conversation);
            return CreatedAtAction(nameof(GetConversation), new { conversationId = createdConversation.Id }, createdConversation);
        }

        [HttpPut("{conversationId}")]
        public async Task<IActionResult> UpdateConversation(int conversationId, Conversation conversation)
        {
            if (conversationId != conversation.Id)
            {
                return BadRequest();
            }

            var updatedConversation = await _conversationRepository.UpdateConversation(conversationId, conversation);
            if (updatedConversation == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{conversationId}")]
        public async Task<IActionResult> DeleteConversation(int conversationId)
        {
            var conversationToDelete = await _conversationRepository.GetConversationById(conversationId);
            if (conversationToDelete == null)
            {
                return NotFound();
            }

            await _conversationRepository.DeleteConversation(conversationId);
            return NoContent();
        }

        [HttpGet("{conversationId}/messages")]
        public async Task<ActionResult<List<Message>>> GetConversationMessages(int conversationId)
        {
            var messages = await _messageRepository.GetMessagesByConversationIdAsync(conversationId);
            if (messages == null)
            {
                return NotFound();
            }
            return Ok(messages);
        }
    }
}
