// Hubs/ChatHub.cs
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

public class ChatHub : Hub
{
    private readonly IRepository<ChatMessage> _repository;

    public ChatHub(IRepository<ChatMessage> repository)
    {
        _repository = repository;
    }

    public async Task SendMessage(string user, string message)
    {
        var chatMessage = new ChatMessage { Sender = user, Message = message };
        _repository.Add(chatMessage);
        await Clients.All.SendAsync("ReceiveMessage", chatMessage);
    }
}
