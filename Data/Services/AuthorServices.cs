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
    
    public AuthorServices(RwaContext context)
    {
        _context = context;
    }
    
    public async Task<Author> Get(int id)
    {
        Author author = await _context.Authors
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.IdAuthor == id);
        if (author == null)
        {
            throw new NotFoundException($"Author with id {id} not found");
        }

        return author;
    }

    public async Task Delete(int id)
    {
        Author author = await _context.Authors.FirstOrDefaultAsync(a => a.IdAuthor == id);
        _context.Authors.Remove(author);
        await _context.SaveChangesAsync();
    }

    public async Task Update(int id, Author author)
    {
        Author original = await _context.Authors.FirstOrDefaultAsync(a => a.IdAuthor == id);
        if (original == null)
        {
            throw new NotFoundException($"author with id {id} not found");
        }

        original.AuthorName = author.AuthorName;
        
        _context.Authors.Update(original);
        await _context.SaveChangesAsync();
    }

    public async Task Create(Author newAuthor)
    {
        Author? author = await _context.Authors.FirstOrDefaultAsync(a => a.IdAuthor == newAuthor.IdAuthor);
        if (author != null)
        {
            throw new AlreadyExistsException($"Author with {newAuthor.IdAuthor} already exists");
        }

        await _context.Authors.AddAsync(newAuthor);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Author>> GetAll()
    {
        return await _context.Authors
            .AsNoTracking()
            .ToListAsync();
    }
}