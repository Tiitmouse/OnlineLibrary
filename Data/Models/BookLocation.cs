using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class BookLocation
{
    public int Id { get; set; }

    public int BookId { get; set; }

    public int LocationId { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual Location Location { get; set; } = null!;

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
