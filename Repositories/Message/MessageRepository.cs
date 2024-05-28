using System.Threading.Tasks;
using TestApi.Models;

public class MessageRepository : IMessageRepository
{
    private readonly AuthContext _context;

    public MessageRepository(AuthContext context)
    {
        _context = context;
    }

    public async Task AddMessage(Message message)
    {
        _context.Messages.Add(message);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Message>> GetMessagesByConversationIdAsync(int conversationId)
    {
        return await _context.Messages
            .Where(m => m.ConversationId == conversationId)
            .ToListAsync();
    }



    // Implement other repository methods as needed
}
