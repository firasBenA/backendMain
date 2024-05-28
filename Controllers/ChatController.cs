using ConversationApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using PrivateChatApp.Hubs;
using System;
using System.Threading.Tasks;
using TestApi.Models;
using TestApi.Repositories;

namespace TestApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IConversationRepository _conversationRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly IHubContext<ChatHub> _chatHubContext;

        public ChatController(IConversationRepository conversationRepository, IMessageRepository messageRepository, IHubContext<ChatHub> chatHubContext)
        {
            _conversationRepository = conversationRepository;
            _messageRepository = messageRepository;
            _chatHubContext = chatHubContext;

        }

        [HttpPost("sendMessage")]
        public async Task<IActionResult> SendMessage(Message message)
        {
            await _messageRepository.AddMessage(message);

            await _chatHubContext.Clients.Group(message.ConversationId.ToString()).SendAsync("ReceiveMessage", message);

            return Ok();
        }

    }
}
