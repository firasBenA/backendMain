using System;
using System.Collections.Generic;

namespace TestApi.Models;

public partial class ChatMessage
{
    public int Id { get; set; }

    public int? IdSender { get; set; }

    public string? Message { get; set; }

    public int? IdReciver { get; set; }

    public string? CreatedAt { get; set; }

    public virtual User? IdReciverNavigation { get; set; }

    public virtual User? IdSenderNavigation { get; set; }
}
