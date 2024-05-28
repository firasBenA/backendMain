using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConversationApi.Repositories
{
    public interface IConversationRepository
    {
        Task<Conversation> GetConversationById(int conversationId);
        Task<Conversation> CreateConversation(Conversation conversation);
        Task<Conversation> UpdateConversation(int conversationId, Conversation conversation);
        Task DeleteConversation(int conversationId);
        Task<Conversation> GetOrCreateConversationAsync(int user1Id, int user2Id);
        Task<IEnumerable<Conversation>> GetAllConversations(); // Add this method

    }
}
