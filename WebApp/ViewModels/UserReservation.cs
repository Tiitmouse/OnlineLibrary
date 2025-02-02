namespace WebApp.Models;

public class UserReservation
{
    public int IdReservation { get; set; }
    public int UserId { get; set; }
    public string LibraryName { get; set; }
    public int BookLocationId { get; set; }
    public string BookTitle { get; set; }
    
    public int LocationId { get; set; }
    public DateTime? ReservationDate { get; set; }
    public bool? Status { get; set; }
}