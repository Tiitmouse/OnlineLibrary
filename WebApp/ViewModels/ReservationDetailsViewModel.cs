namespace WebApp.Models;

public class ReservationDetailsViewModel
{
    public int IdReservation { get; set; }
    public int UserId { get; set; }
    public string CosumerName { get; set; }
    public int BookLocationId { get; set; }
    public string BookTitle { get; set; }
    public DateTime? ReservationDate { get; set; }
    public bool? Status { get; set; }
}