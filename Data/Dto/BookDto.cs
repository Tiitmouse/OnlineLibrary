namespace Data.Dto;

public class BookDto
{
    public int IdBook { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public int? PublicationYear { get; set; }
    public string Isbn { get; set; } 
    public int AuthorId { get; set; }
    public string AuthorName { get; set; }
    public int GenreId { get; set; }
    public string GenreName { get; set; }
}