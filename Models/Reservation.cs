using System;
using System.Collections.Generic;

namespace TestApi.Models;

public partial class Reservation
{
    public int Id { get; set; }

    public string? DateDebut { get; set; }

    public double? PrixTotale { get; set; }

    public int? IdBoat { get; set; }

    public string? DateFin { get; set; }

    public string? RéservantName { get; set; }

    public int? IdUser { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public virtual Boat? IdBoatNavigation { get; set; }
}
