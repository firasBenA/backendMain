using System.Collections.Generic;
using TestApi.Models;

namespace TestApi.Repositories
{
    public interface IChatMessageRepository
    {
        IEnumerable<ChatMessage> GetMessages();
        void AddMessage(ChatMessage message);
        Task<IEnumerable<ChatMessage>> GetMessagesAsync(int userId1, int userId2);

    }
}
