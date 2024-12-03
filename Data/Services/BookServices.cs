using Data.Exceptions;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Services;

public interface IBookService
{
    public Task<Book> Get(int id);
    public Task Delete(int id);
    public void Update(int id, Book book);
    public void Create(Book newBook);
    public Task<List<Book>> GetAll();
}

public class BookServices : IBookService
{
    private readonly OnlineLibraryContext _context;

    public BookServices(OnlineLibraryContext context)
    {
        _context = context;
    }

    public async Task<Book> Get(int id)
    {
        Book book = await _context.Books
            .AsNoTracking()
            .Include(b => b.Genres)
            .Include(b => b.Author)
            .Include(b => b.Locations)
            .FirstOrDefaultAsync(b => b.IdBook == id);
        if (book == null)
        {
            throw new NotFoundException($"Book with id {id} not found");
        }

        return book;
    }

    public async Task Delete(int id)
    {
        Book book = await _context.Books.FirstOrDefaultAsync(b => b.IdBook == id);
        _context.Books.Remove(book);
        await _context.SaveChangesAsync();
    }

    public async void Update(int id, Book book)
    {
        Book original = await _context.Books.FirstOrDefaultAsync(b => b.IdBook == id);
        if (original == null)
        {
            throw new NotFoundException($"Book with id {id} not found");
        }

        original.Title = book.Title;
        original.Description = book.Description;
        original.PublicationYear = book.PublicationYear;
        original.Isbn = book.Isbn;

        _context.Books.Update(original);
        await _context.SaveChangesAsync();
    }

    public async void Create(Book newBook)
    {
        Book? book = _context.Books.FirstOrDefault(b => b.Isbn == newBook.Isbn);
        if (book != null)
        {
            throw new AlreadyExistsException($"Book with {newBook.Isbn} already exists");
        }

        await _context.Books.AddAsync(newBook);
        await _context.SaveChangesAsync();

    }

    public async Task<List<Book>> GetAll()
    {
        return await _context.Books
            .AsNoTracking()
            .Include(b => b.Genres)
            .ToListAsync();
    }
}