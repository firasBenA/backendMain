using System;
using System.Collections.Generic;

namespace TestApi.Models;

public partial class User
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public int? IdBoat { get; set; }

    public int? IdChat { get; set; }

    public string? Avatar { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Country { get; set; }

    public string? City { get; set; }

    public string? StreetAddress { get; set; }

    public int? IdRole { get; set; }

    public int? Active { get; set; }

    public virtual ICollection<FeedBack> FeedBacks { get; set; } = new List<FeedBack>();

    public virtual Role? IdRoleNavigation { get; set; }
}
