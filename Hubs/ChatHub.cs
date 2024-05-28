using ConversationApi.Repositories;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;
using TestApi.Models;
using TestApi.Repositories;

namespace PrivateChatApp.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IConversationRepository _conversationRepository;
        private readonly IMessageRepository _messageRepository;

        public ChatHub(IConversationRepository conversationRepository, IMessageRepository messageRepository)
        {
            _conversationRepository = conversationRepository;
            _messageRepository = messageRepository;
        }

        public async Task SendMessage(string senderId, string receiverId, string messageContent)
        {
            if (!int.TryParse(senderId, out int senderIntId) || !int.TryParse(receiverId, out int receiverIntId))
            {
                throw new ArgumentException("Invalid sender or receiver ID.");
            }

            var conversation = await _conversationRepository.GetOrCreateConversationAsync(senderIntId, receiverIntId);

            var message = new Message
            {
                SenderId = senderIntId.ToString(),
                ConversationId = conversation.Id,
                Text = messageContent,
                Timestamp = DateTime.UtcNow
            };

            await _messageRepository.AddMessage(message);

            // Notify clients in the conversation group
            await Clients.Group(conversation.Id.ToString()).SendAsync("ReceiveMessage", message);
        }

        public async Task JoinConversation(string conversationId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, conversationId);
        }

        public async Task LeaveConversation(string conversationId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, conversationId);
        }
    }
}
