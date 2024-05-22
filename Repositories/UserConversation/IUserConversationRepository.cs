using System.Threading.Tasks;

namespace UserConversationApi.Repositories
{
    public interface IUserConversationRepository
    {
        Task<IEnumerable<UserConversation>> GetUserConversationsByUserId(int userId);
        // Add other methods as needed
    }
}
