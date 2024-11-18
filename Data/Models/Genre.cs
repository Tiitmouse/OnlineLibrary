using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class Genre
{
    public int IdGenre { get; set; }

    public string GenreName { get; set; } = null!;

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
