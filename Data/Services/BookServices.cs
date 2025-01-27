using Data.Dto;
using Data.Eunumerators;
using Data.Exceptions;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Services;

public interface IBookService
{
    public Task<Book> Get(int id);
    public Task Delete(int id);
    public Task DeleteByAuthorId(int authorId);
    public Task DeleteByGenreId(int genreId);
    public Task Update(int id, Book book);
    public Task Create(Book newBook);
    public Task<List<Book>> GetAll();
    public Task<List<Book>> SearchAndPaginate(string seachTerm, int n, int page);
}

public class BookServices : IBookService
{
    private readonly RwaContext _context;
    private readonly ILogService _logService;

    public BookServices(RwaContext context, ILogService logService)
    {
        _context = context;
        _logService = logService;
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
            await _logService.Create("Failed to fetch book with ID {id}, because there is none with the same ID", Importance.Low);
            throw new NotFoundException($"Book with id {id} not found");
        }
        await _logService.Create("Book with ID {id} fetched successdully", Importance.Low);
        return book;
    }

    public async Task Delete(int id)
    {
        Book book = await _context.Books.FirstOrDefaultAsync(b => b.IdBook == id);
        if (book == null)
        {
            await _logService.Create("Cannot delete book with ID {id}, because there is none with the same ID", Importance.Low);
            throw new NotFoundException($"Book with id {id} not found");
        }
        _context.Books.Remove(book);
        await _logService.Create("Book with ID {id} successfully deleted", Importance.High);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteByAuthorId(int authorId)
    {
        var books = await _context.Books.Where(b => b.AuthorId == authorId).ToListAsync();
        if (books == null || !books.Any())
        {
            await _logService.Create($"No books found for author with ID {authorId}", Importance.Low);
            return;
        }
        _context.Books.RemoveRange(books);
        await _logService.Create($"All books for author with ID {authorId} successfully deleted", Importance.High);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteByGenreId(int genreId)
    {
        var books = await _context.Books.Where(b => b.GenreId == genreId).ToListAsync();
        if (books == null || !books.Any())
        {
            await _logService.Create($"No books found for genre with ID {genreId}", Importance.Low);
            return;
        }
        _context.Books.RemoveRange(books);
        await _logService.Create($"All books for genre with ID {genreId} successfully deleted", Importance.High);
        await _context.SaveChangesAsync();
    }

    public async Task Update(int id, Book book)
    {
        Book original = await _context.Books.FirstOrDefaultAsync(b => b.IdBook == id);
        if (original == null)
        {
            await _logService.Create("Cannot update book with ID {id}, because there is none with the same ID", Importance.Low);
            throw new NotFoundException($"Book with id {id} not found");
        }

        original.Title = book.Title;
        original.Description = book.Description;
        original.PublicationYear = book.PublicationYear;
        original.Isbn = book.Isbn;
        original.GenreId = book.GenreId;
        original.AuthorId = book.AuthorId;

        _context.Books.Update(original);
        await _logService.Create("Book with ID {id} successfully updated", Importance.High);
        await _context.SaveChangesAsync();
    }

    public async Task Create(Book newBook)
    {
        Book? book = await _context.Books.FirstOrDefaultAsync(b => b.Isbn == newBook.Isbn);
        if (book != null)
        {
            await _logService.Create("Cannot create book with ISBN {newBook.Isbn}, because there is already a book with the same ISBN", Importance.Low);
            throw new AlreadyExistsException($"Book with {newBook.Isbn} already exists");
        }

        await _context.Books.AddAsync(newBook);
        await _logService.Create("Book with ISBN {newBook.Isbn} successfully added", Importance.High);
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

    public async Task<List<Book>> SearchAndPaginate(string seachTerm, int n, int page)
    {
        return await _context.Books
            .AsNoTracking()
            .Include(b => b.Genre)
            .Include(b => b.BookLocations)
            .Include(b => b.Author)
            .Where(b => string.IsNullOrEmpty(seachTerm) || 
                        b.Title.ToLower().Contains(seachTerm) ||
                        (b.Author != null && b.Author.AuthorName.ToLower().Contains(seachTerm)) ||
                        (b.Genre != null && b.Genre.GenreName.ToLower().Contains(seachTerm)))
            .Skip(n * (page - 1))
            .Take(n)
            .ToListAsync();
    }
}