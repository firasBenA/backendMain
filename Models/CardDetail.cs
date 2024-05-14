using System;
using System.Collections.Generic;

namespace TestApi.Models;

public partial class CardDetail
{
    public string? CardNumber { get; set; }

    public string? ExpiryMonth { get; set; }

    public string? ExpiryYear { get; set; }

    public string? Cvv { get; set; }
}
