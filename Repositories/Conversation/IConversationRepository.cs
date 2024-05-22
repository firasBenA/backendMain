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
    }
}
