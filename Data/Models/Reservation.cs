﻿using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class Reservation
{
    public int IdReservation { get; set; }

    public int? UserId { get; set; }

    public int? BookId { get; set; }

    public DateTime? ReservationDate { get; set; }

    public string? Status { get; set; }

    public virtual Book? Book { get; set; }

    public virtual User? User { get; set; }
}
