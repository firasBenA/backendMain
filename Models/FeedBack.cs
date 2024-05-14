using System;
using System.Collections.Generic;

namespace TestApi.Models;

public partial class FeedBack
{
    public int Id { get; set; }

    public int? Rating { get; set; }

    public string? Comment { get; set; }

    public int? IdBoat { get; set; }

    public int? IdUser { get; set; }

    public virtual User? IdUserNavigation { get; set; }
}
