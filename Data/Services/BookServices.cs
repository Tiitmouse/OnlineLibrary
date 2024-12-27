using Data.Exceptions;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Services;

public interface IBookService
{
    public Task<Book> Get(int id);
    public Task Delete(int id);
    public Task Update(int id, Book book);
    public Task Create(Book newBook);
    public Task<List<Book>> GetAll();
}

public class BookServices : IBookService
{
    private readonly RwaContext _context;

    public BookServices(RwaContext context)
    {
        _context = context;
    }

    public async Task<Book> Get(int id)
    {
        Book book = await _context.Books
            .AsNoTracking()
            .Include(b => b.Genre)
            .Include(b => b.BookLocations)
            .Include(b => b.Author)
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

    public async Task Update(int id, Book book)
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

    public async Task Create(Book newBook)
    {
        Book? book = await _context.Books.FirstOrDefaultAsync(b => b.Isbn == newBook.Isbn);
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
            .Include(b => b.Genre)
            .Include(b => b.BookLocations)
            .Include(b => b.Author)
            .ToListAsync();
    }
}