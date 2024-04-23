using System;
using System.Collections.Generic;

namespace TestApi.Models;

public partial class Boat
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? Capacity { get; set; }

    public int? NbrCabins { get; set; }

    public int? NbrBedrooms { get; set; }

    public string? Description { get; set; }

    public int? Price { get; set; }

    public string? Type { get; set; }

    public int? PhoneNumber { get; set; }

    public int? IdFeedBack { get; set; }

    public string? ImageUrl { get; set; }
}
