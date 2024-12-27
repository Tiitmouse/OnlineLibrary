using Data.Exceptions;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Services;

public interface IGenreService
{
    public Task<Genre> Get(int id);
    public Task Delete(int id);
    public Task Update(int id, Genre genre);
    public Task Create(Genre genre);
    public Task<List<Genre>> GetAll();
}

public class GenreService : IGenreService
{
    private readonly RwaContext _context;
    
    public GenreService(RwaContext context)
    {
        _context = context;
    }
    
    public async Task<Genre> Get(int id)
    {
        Genre genre = await _context.Genres
            .AsNoTracking()
            .FirstOrDefaultAsync(g => g.IdGenre == id);
        if (genre == null)
        {
            throw new NotFoundException($"Genre with id {id} not found");
        }

        return genre;
    }

    public async Task Delete(int id)
    {
        Genre genre = await _context.Genres.FirstOrDefaultAsync(b => b.IdGenre == id);
        _context.Genres.Remove(genre);
        await _context.SaveChangesAsync();
    }

    public async Task Update(int id, Genre genre)
    {
        Genre original = await _context.Genres.FirstOrDefaultAsync(g => g.IdGenre == id);
        if (original == null)
        {
            throw new NotFoundException($"Genre with id {id} not found");
        }
        original.GenreName = genre.GenreName;
        _context.Genres.Update(original);
        await _context.SaveChangesAsync();
    }

    public async Task Create(Genre newgenre)
    {
        Genre? genre = await _context.Genres.FirstOrDefaultAsync(g => g.GenreName == newgenre.GenreName);
        if (genre != null)
        {
            throw new AlreadyExistsException($"genre with {newgenre.GenreName} already exists");
        }
        await _context.Genres.AddAsync(newgenre);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Genre>> GetAll()
    {
        return await _context.Genres
            .AsNoTracking()
            .ToListAsync();
    }
}