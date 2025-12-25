using System;
using System.Collections.Generic;

namespace CINEMA_WEB3.Models;

public partial class Theater
{
    public int TheaterId { get; set; }

    public string? Name { get; set; }

    public int? SeatCapacity { get; set; }

    public virtual ICollection<Showtime> Showtimes { get; set; } = new List<Showtime>();
}
