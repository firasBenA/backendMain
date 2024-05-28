using System.Collections.Generic;
using TestApi.Models;

namespace TestApi.Repositories
{
    public interface IChatMessageRepository
    {
        void AddMessage(ChatMessage message);
        Task<List<ChatMessage>> SendMessage(int senderUserId, int receiverUserId, string message);
        Task<IEnumerable<ChatMessage>> GetMessagesAsync(int senderUserId, int receiverUserId);

    }
}
