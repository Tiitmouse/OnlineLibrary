using System.Dynamic;

namespace Data.Dto;

public class BookDetailsDto
{
    public int IdBook { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public int? PublicationYear { get; set; }
    public string Isbn { get; set; } 
    public int AuthorId { get; set; }
    public GenreDto Genre { get; set; }
    public List<LocationDto> Locations { get; set; }
}