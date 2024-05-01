using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


public class ChatMessageRepository : IChatMessageRepository
{
    private readonly AuthContext _context;

    public ChatMessageRepository(AuthContext context)
    {
        _context = context;
    }

    public async Task SendMessage(string message)
    {
        
        Console.WriteLine($"Message sent: {message}");
    }
}

