namespace Data.Dto;

public class LocationDto
{
    public int IdLocation { get; set; }
    public string LocationName { get; set; } = null!;
    public string? Address { get; set; }
}