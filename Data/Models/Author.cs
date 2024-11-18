using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class Author
{
    public int IdAuthor { get; set; }

    public string AuthorName { get; set; } = null!;

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
