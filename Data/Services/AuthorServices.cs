using Data.Eunumerators;
using Data.Exceptions;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Services;

public interface IAuthorService
{
    public Task<Author> Get(int id);
    public Task Delete(int id);
    public Task Update(int id, Author author);
    public Task Create(Author author);
    public Task<List<Author>> GetAll();
}

public class AuthorServices : IAuthorService
{
    private readonly RwaContext _context;
    private readonly ILogService _logService;
    
    public AuthorServices(RwaContext context, ILogService logService)
    {
        _context = context;
        _logService = logService;
    }
    
    public async Task<Author> Get(int id)
    {
        Author author = await _context.Authors
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.IdAuthor == id);
        if (author == null)
        {
            await _logService.Create("Failed to fetch autor with ID {id}, because there is none with the same ID", Importance.Low);
            throw new NotFoundException($"Author with id {id} not found");
        }
        await _logService.Create("Author with ID {id} fetched successfully", Importance.Low);
        return author;
    }

    public async Task Delete(int id)
    {
        Author author = await _context.Authors.FirstOrDefaultAsync(a => a.IdAuthor == id);
        if (author == null)
        {
            await _logService.Create("Cannot delete author with ID {id}, because there is none with the same ID", Importance.Low);
            throw new NotFoundException($"author with id {id} not found");
        }
        _context.Authors.Remove(author);
        await _logService.Create("Author with ID {id} successfully deleted", Importance.High);
        await _context.SaveChangesAsync();
    }

    public async Task Update(int id, Author author)
    {
        Author original = await _context.Authors.FirstOrDefaultAsync(a => a.IdAuthor == id);
        if (original == null)
        {
            await _logService.Create("Cannot update author with ID {id}, because there is none with the same ID", Importance.Low);
            throw new NotFoundException($"author with id {id} not found");
        }

        original.AuthorName = author.AuthorName;
        
        _context.Authors.Update(original);
        await _logService.Create("Author with ID {id} auccessfully updated", Importance.Medium);
        await _context.SaveChangesAsync();
    }

    public async Task Create(Author newAuthor)
    {
        Author? author = await _context.Authors.FirstOrDefaultAsync(a => a.IdAuthor == newAuthor.IdAuthor);
        if (author != null)
        {
            await _logService.Create($"Failed to add new author, because author with ID {newAuthor.IdAuthor} already existst", Importance.Low);
            throw new AlreadyExistsException($"Author with {newAuthor.IdAuthor} already exists");
        }

        await _context.Authors.AddAsync(newAuthor);
        await _logService.Create($"Author with ID {newAuthor.IdAuthor} successfully added", Importance.Low);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Author>> GetAll()
    {
        return await _context.Authors
            .AsNoTracking()
            .ToListAsync();
    }
}