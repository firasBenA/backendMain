using System;
using System.Collections.Generic;

namespace TestApi.Models;

public partial class ChatMessage
{
    public string? Message { get; set; }

    public string? Sender { get; set; }
}
