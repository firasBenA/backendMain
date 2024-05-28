using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ConversationApi.Repositories
{
    public class ConversationRepository : IConversationRepository
    {
        private readonly AuthContext _context;

        public ConversationRepository(AuthContext context)
        {
            _context = context;
        }

        public async Task<Conversation> GetConversationById(int conversationId)
        {
            return await _context.Conversations
                .FirstOrDefaultAsync(c => c.Id == conversationId);
        }

        public async Task<Conversation> CreateConversation(Conversation conversation)
        {
            if (conversation == null)
            {
                throw new ArgumentNullException(nameof(conversation));
            }

            _context.Conversations.Add(conversation);
            await _context.SaveChangesAsync();

            return conversation;
        }

        public async Task<Conversation> UpdateConversation(int conversationId, Conversation conversation)
        {
            var existingConversation = await _context.Conversations.FindAsync(conversationId);

            if (existingConversation == null)
            {
                return null;
            }

            existingConversation.GroupName = conversation.GroupName;
            // Update other properties as needed

            await _context.SaveChangesAsync();

            return existingConversation;
        }

        public async Task DeleteConversation(int conversationId)
        {
            var conversationToDelete = await _context.Conversations.FindAsync(conversationId);

            if (conversationToDelete != null)
            {
                _context.Conversations.Remove(conversationToDelete);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Conversation> GetOrCreateConversationAsync(int user1Id, int user2Id)
        {
            var existingConversation = await _context.Conversations.FirstOrDefaultAsync(c =>
                (c.User1Id == user1Id && c.User2Id == user2Id) ||
                (c.User1Id == user2Id && c.User2Id == user1Id));

            if (existingConversation != null)
            {
                return existingConversation;
            }

            var newConversation = new Conversation
            {
                User1Id = user1Id,
                User2Id = user2Id
            };
            _context.Conversations.Add(newConversation);
            await _context.SaveChangesAsync();

            return newConversation;
        }
        public async Task<IEnumerable<Conversation>> GetAllConversations()
        {
            return await _context.Conversations.ToListAsync();
        }
    }
}
