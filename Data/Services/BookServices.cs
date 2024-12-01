using Data.Exceptions;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Services;

public interface IBookService 
{
    public Task<Book> Get(int id);
    public void Delete(int id);
    public void Update(int id, Book book);
    public void Create(Book book);
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
        Book book = await _context.Books.FirstOrDefaultAsync(b => b.IdBook == id);
        if (book == null)
        {
            throw new NotFoundException($"Book with id {id} not found");
        }
        return book;
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public void Update(int id, Book book)
    {
        throw new NotImplementedException();
    }

    public void Create(Book book)
    {
        throw new NotImplementedException();
    }
}

