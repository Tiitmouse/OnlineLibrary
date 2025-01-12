namespace WebApp.Models;

public class LibraryAvailabilityViewModel
{
    public int LocationId { get; set; }
    public int BookLocationId { get; set; }
    public string LocationName { get; set; }   
    public string LocationAddress { get; set; }   
    public bool IsAvailable { get; set; }
}