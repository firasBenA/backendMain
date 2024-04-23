using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestApi.Models;

namespace TestApi.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private readonly AuthContext _context;

        public ChatRepository(AuthContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Chat>> GetAllChatsAsync()
        {
            return await _context.Chats.ToListAsync();
        }

        public async Task<Chat> GetChatByIdAsync(int id)
        {
            return await _context.Chats.FindAsync(id);
        }

        public async Task AddChatAsync(Chat chat)
        {
            _context.Chats.Add(chat);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateChatAsync(Chat chat)
        {
            _context.Entry(chat).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteChatAsync(int id)
        {
            var chat = await _context.Chats.FindAsync(id);
            _context.Chats.Remove(chat);
            await _context.SaveChangesAsync();
        }
    }
}
