namespace Data.Dto;

public class NewBookDto
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public int? PublicationYear { get; set; }
    public string Isbn { get; set; } 
    public int AuthorId { get; set; }
    public int GenreId { get; set; }
    public List<int> LocationsId { get; set; }
}