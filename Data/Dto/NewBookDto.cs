namespace Data.Dto;

public class NewBookDto
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public int? PublicationYear { get; set; }
    public string Isbn { get; set; } 
    public int AuthorId { get; set; }
    public List<int> Genres { get; set; }
    public List<int> Locations { get; set; }
}