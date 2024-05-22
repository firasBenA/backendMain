// ChatHub.cs
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;
namespace PrivateChatApp.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IUserRepository _userRepository;
        private readonly IChatMessageRepository _messageRepository;

        public ChatHub(IUserRepository userRepository, IChatMessageRepository messageRepository)
        {
            _userRepository = userRepository;
            _messageRepository = messageRepository;
        }

        public async Task SendMessage(int receiverUserId, string message)
        {
            var senderUserId = Context.UserIdentifier;
            var receiver = await _userRepository.GetByIdAsync(receiverUserId);

            if (receiver != null)
            {
                var newMessage = new ChatMessage
                {
                    IdSender = int.Parse(senderUserId), 
                    Message = message,
                    IdReciver = receiverUserId,
                    CreatedAt = DateTime.UtcNow.ToString()
                };
                _messageRepository.AddMessage(newMessage);

                await Clients.User(receiver.Id.ToString()).SendAsync("ReceiveMessage", senderUserId, message);
            }
            else
            {
                throw new ArgumentException("Receiver not found.");
            }
        }
    }
}
