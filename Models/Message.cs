using System;
using System.Collections.Generic;

namespace TestApi.Models;

public partial class Message
{
    public int Id { get; set; }

    public int? ConversationId { get; set; }

    public string? SenderId { get; set; }

    public string? Text { get; set; }

    public DateTime? Timestamp { get; set; }

    public virtual Conversation? Conversation { get; set; }
}
