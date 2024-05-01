using System;
using System.Collections.Generic;

namespace TestApi.Models;

public partial class ChatMessage
{
    public int Id { get; set; }

    public string? Message { get; set; }

    public string? User { get; set; }

    public DateTime? Timestamp { get; set; }
}
