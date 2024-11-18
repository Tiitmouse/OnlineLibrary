using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class Book
{
    public int IdBook { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public int? PublicationYear { get; set; }

    public string? Isbn { get; set; }

    public int? AuthorId { get; set; }

    public virtual Author? Author { get; set; }

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

    public virtual ICollection<Genre> Genres { get; set; } = new List<Genre>();

    public virtual ICollection<Location> Locations { get; set; } = new List<Location>();
}
