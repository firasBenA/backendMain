using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestApi.Models;

namespace TestApi.Repositories
{
    public class ChatMessageRepository : IChatMessageRepository
    {
        private readonly AuthContext _context;

        public ChatMessageRepository(AuthContext context)
        {
            _context = context;
        }

        public void AddMessage(ChatMessage message)
        {
            _context.ChatMessages.Add(message);
            _context.SaveChanges();
        }

        public async Task<List<ChatMessage>> SendMessage(int senderUserId, int receiverUserId, string message)
        {
            return await _context.ChatMessages
                .Where(m => (m.IdSender == senderUserId && m.IdReciver == receiverUserId) || (m.IdSender == receiverUserId && m.IdReciver == senderUserId))
                .ToListAsync();
        }

        public Task<IEnumerable<ChatMessage>> GetMessagesAsync(int senderUserId, int receiverUserId)
        {
            // Assuming _messages contains all messages and you want to filter based on senderUserId and receiverUserId
            var filteredMessages = _context.ChatMessages.Where(msg => msg.IdSender == senderUserId && msg.IdReciver == receiverUserId).ToList();
            return Task.FromResult<IEnumerable<ChatMessage>>(filteredMessages);
        }

        
    }
}
