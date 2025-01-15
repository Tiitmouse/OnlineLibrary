namespace WebApp.Models;

public class LocationViewModel
{
    public int IdLocation { get; set; }
    public string LocationName { get; set; } = null!;

    public string? Address { get; set; }
}