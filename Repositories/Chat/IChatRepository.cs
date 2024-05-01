using System.Collections.Generic;
using System.Threading.Tasks;
using TestApi.Models;

namespace TestApi.Repositories
{
    public interface IChatRepository
    {
        Task<IEnumerable<Chat>> GetAllChatsAsync();
        Task<Chat> GetChatByIdAsync(int id);
        Task AddChatAsync(Chat chat);
        Task UpdateChatAsync(Chat chat);
        Task DeleteChatAsync(int id);
    }
}
