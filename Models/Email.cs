using System;
using System.Collections.Generic;

namespace TestApi.Models;

public partial class Email
{
    public int? Id { get; set; }

    public string? To { get; set; }

    public string? Subject { get; set; }

    public string? Body { get; set; }
}
