using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class Rating
{
    public int IdRating { get; set; }

    public int? UserId { get; set; }

    public int? BookId { get; set; }

    public int? Rating1 { get; set; }

    public string? Comment { get; set; }

    public virtual Book? Book { get; set; }

    public virtual User? User { get; set; }
}
