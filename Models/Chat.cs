using System;
using System.Collections.Generic;

namespace TestApi.Models;

public partial class Chat
{
    public int Id { get; set; }

    public string? Message { get; set; }

    public DateTime? Date { get; set; }
}
