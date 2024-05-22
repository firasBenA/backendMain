using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConversationApi.Repositories
{
    public class ConversationRepository : IConversationRepository
    {
        private readonly List<Conversation> _conversations;



        public Task<Conversation> GetConversationById(int conversationId)
        {
            var conversation = _conversations.FirstOrDefault(c => c.Id == conversationId);
            return Task.FromResult(conversation);
        }

        public Task<Conversation> CreateConversation(Conversation conversation)
        {
            if (conversation == null)
            {
                throw new ArgumentNullException(nameof(conversation));
            }

            conversation.Id = _conversations.Any() ? _conversations.Max(c => c.Id) + 1 : 1;
            _conversations.Add(conversation);

            return Task.FromResult(conversation);
        }


        public Task<Conversation> UpdateConversation(int conversationId, Conversation conversation)
        {
            var existingConversation = _conversations.FirstOrDefault(c => c.Id == conversationId);
            if (existingConversation == null)
            {
                return Task.FromResult<Conversation>(null);
            }

            existingConversation.GroupName = conversation.GroupName;

            return Task.FromResult(existingConversation);
        }

        public Task DeleteConversation(int conversationId)
        {
            var conversationToDelete = _conversations.FirstOrDefault(c => c.Id == conversationId);
            if (conversationToDelete != null)
            {
                _conversations.Remove(conversationToDelete);
            }

            return Task.CompletedTask;
        }

        // Add other methods as needed
    }
}
