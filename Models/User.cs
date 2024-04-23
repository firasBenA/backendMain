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

    public int? IdRole { get; set; }

    public int? IdChat { get; set; }
}
