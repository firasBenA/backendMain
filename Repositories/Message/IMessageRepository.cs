using System.Threading.Tasks;
using TestApi.Models;

public interface IMessageRepository
{
    Task AddMessage(Message message);
    Task<List<Message>> GetMessagesByConversationIdAsync(int conversationId);
}
