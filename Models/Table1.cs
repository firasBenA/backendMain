using System;
using System.Collections.Generic;

namespace TestApi.Models;

public partial class Table1
{
    public int Id { get; set; }

    public int? ConversationId { get; set; }

    public string? Conversation { get; set; }

    public string? SenderId { get; set; }

    public string? Sender { get; set; }

    public string? Text { get; set; }

    public int? Timestamp { get; set; }
}
