using System;
using System.Collections.Generic;

namespace TestApi.Models;

public partial class Conversation
{
    public int Id { get; set; }

    public int? User1Id { get; set; }

    public int? User2Id { get; set; }

    public string? GroupName { get; set; }

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    public virtual User? User2 { get; set; }
}
