using System;
using System.Collections.Generic;

namespace TestApi.Models;

public partial class FeedBack
{
    public int Id { get; set; }

    public string? Comment { get; set; }

    public int? Rating { get; set; }
}
