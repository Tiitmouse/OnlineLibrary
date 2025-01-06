namespace WebApp.Models;

public class BookViewModel
{
    public int IdBook { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public int? PublicationYear { get; set; }
    public string Isbn { get; set; } 
    public string AuthorName { get; set; }
    public string GenreName { get; set; }
}