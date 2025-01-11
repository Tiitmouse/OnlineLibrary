using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class Book
{
    public int IdBook { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public int? PublicationYear { get; set; }

    public string Isbn { get; set; } = null!;

    public int? AuthorId { get; set; }

    public int? GenreId { get; set; }

    public virtual Author? Author { get; set; }

    public virtual ICollection<BookLocation> BookLocations { get; set; } = new List<BookLocation>();

    public virtual Genre? Genre { get; set; }
}
