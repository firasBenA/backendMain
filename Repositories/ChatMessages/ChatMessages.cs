using System;
using System.Collections.Generic;
using System.Linq;
using TestApi.Models;

namespace TestApi.Repositories
{
    public class ChatMessagesRepository : IChatMessageRepository
    {
        private readonly AuthContext _dbContext;

        public ChatMessagesRepository(AuthContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<ChatMessage> GetMessages()
        {
            return _dbContext.ChatMessages.OrderByDescending(m => m.CreatedAt);
        }

        public void AddMessage(ChatMessage message)
        {
            _dbContext.ChatMessages.Add(message);
            _dbContext.SaveChanges();
        }

        public IEnumerable<ChatMessage> GetMessages(int userId1, int userId2)
        {
            return _dbContext.ChatMessages.Where(m => (m.IdSender == userId1 && m.IdReciver == userId2) ||
                                        (m.IdSender == userId2 && m.IdReciver == userId1));
        }

         public async Task<IEnumerable<ChatMessage>> GetMessagesAsync(int userId1, int userId2)
    {
        return await _dbContext.ChatMessages
            .Where(m => (m.IdSender == userId1 && m.IdReciver == userId2) ||
                        (m.IdSender == userId2 && m.IdReciver == userId1))
            .ToListAsync();
    }
    }
}
