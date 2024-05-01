using System.Collections.Generic;
using System.Threading.Tasks;

public interface IChatMessageRepository
{
    Task SendMessage(string message);
}
