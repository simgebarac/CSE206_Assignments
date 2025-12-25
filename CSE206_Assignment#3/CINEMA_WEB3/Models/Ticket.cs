using System;
using System.Collections.Generic;

namespace CINEMA_WEB3.Models;

public partial class Ticket
{
    public int TicketId { get; set; }

    public int? UserId { get; set; }

    public int? ShowtimeId { get; set; }

    public string? SeatNumber { get; set; }

    public DateTime? PurchaseDate { get; set; }

    public virtual Showtime? Showtime { get; set; }

    public virtual User? User { get; set; }
}
