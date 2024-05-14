using System;
using System.Collections.Generic;

namespace TestApi.Models;

public partial class CardDetails
{
    public string? CardNumber { get; set; }

    public int? ExpiryMonth { get; set; }

    public int? ExpiryYear { get; set; }

    public string? Cvv { get; set; }
}
