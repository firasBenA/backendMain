using System;
using System.Collections.Generic;

namespace TestApi.Models;

public partial class EmailVerificationModel
{
    public string? Email { get; set; }

    public string? VerificationCode { get; set; }
}
