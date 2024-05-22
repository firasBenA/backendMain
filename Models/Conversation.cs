using System;
using System.Collections.Generic;

namespace TestApi.Models;

public partial class Conversation
{
    public int Id { get; set; }

    public string? GroupName { get; set; }

    public virtual ICollection<UserConversation> UserConversations { get; set; } = new List<UserConversation>();
}
