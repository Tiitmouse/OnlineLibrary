using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class Location
{
    public int IdLocation { get; set; }

    public string LocationName { get; set; } = null!;

    public string? Address { get; set; }

    public virtual ICollection<BookLocation> BookLocations { get; set; } = new List<BookLocation>();
}
