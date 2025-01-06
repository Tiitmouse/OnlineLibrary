namespace WebApp.Models;

public class BooksTableViewModel
{
    public List<BookViewModel> Books { get; set; }
    public int BookCount;
    public int CurrentPage;
    public int BooksPerPage;
    public string CurrentFilter;
}