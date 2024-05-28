using System;
using System.Collections.Generic;

namespace TestApi.Models;

public partial class UserConversation
{
    public int Id { get; set; }

    public int? IdConversation { get; set; }

    public int? UserId { get; set; }

    public virtual User? User { get; set; }
}
