using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class Log
{
    public int IdLog { get; set; }

    public DateTime Date { get; set; }

    public string Message { get; set; } = null!;

    public int Importance { get; set; }
}
