namespace WebApp.Models;

public class ReservationViewModel
{
    public int IdReservation { get; set; }

    public int? UserId { get; set; }
    
    public int BookId { get; set; }
    
    public int LocationId { get; set; }

    public DateTime? ReservationDate { get; set; }

    public bool? Status { get; set; }
}