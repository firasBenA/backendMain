using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using UserConversationApi.Repositories;

namespace EmailApi.Repositories
{
    public class UserConversationRepository : IUserConversationRepository
    {
        private readonly AuthContext _context;

        public UserConversationRepository(AuthContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserConversation>> GetUserConversationsByUserId(int userId)
        {
            return await _context.UserConversations
                .Include(uc => uc.IdConversation)
                .Where(uc => uc.UserId == userId)
                .ToListAsync();
        }
        // Implement other methods
    }

}
