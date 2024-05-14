using System;
using System.Collections.Generic;

namespace TestApi.Models;

public partial class Equipment
{
    public int Id { get; set; }

    public int? Wifi { get; set; }

    public int? Tv { get; set; }

    public int? HotWater { get; set; }

    public int? Gps { get; set; }

    public int? BathingLadder { get; set; }

    public int? Pilot { get; set; }

    public int? Shower { get; set; }

    public int? Speaker { get; set; }

    public int? IdBoat { get; set; }

    public virtual Boat? IdBoatNavigation { get; set; }
}
