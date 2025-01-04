using Data.Eunumerators;
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
    private readonly ILogService _logService;
    
    public GenreService(RwaContext context, ILogService logService)
    {
        _context = context;
        _logService = logService;
    }
    
    public async Task<Genre> Get(int id)
    {
        Genre genre = await _context.Genres
            .AsNoTracking()
            .FirstOrDefaultAsync(g => g.IdGenre == id);
        if (genre == null)
        {
            _logService.Create("Failed to fetch genre with ID {id}, because there is none with the same ID", Importance.Low);
            throw new NotFoundException($"Genre with id {id} not found");
        }
        _logService.Create("Genre with ID {id} fetched successfully", Importance.Low);
        return genre;
    }

    public async Task Delete(int id)
    {
        Genre genre = await _context.Genres.FirstOrDefaultAsync(b => b.IdGenre == id);
        if (genre == null)
        {
            _logService.Create("Cannot delete genre with ID {id}, because there is none with the same ID", Importance.Low);
            throw new NotFoundException($"genre with id {id} not found");
        }
        _context.Genres.Remove(genre);
        _logService.Create("Genre with ID {id} successfully deleted", Importance.High);
        await _context.SaveChangesAsync();
    }

    public async Task Update(int id, Genre genre)
    {
        Genre original = await _context.Genres.FirstOrDefaultAsync(g => g.IdGenre == id);
        if (original == null)
        {
            _logService.Create("Cannot update genre with ID {id}, because there is none with the same ID", Importance.Low);
            throw new NotFoundException($"Genre with id {id} not found");
        }
        original.GenreName = genre.GenreName;
        _context.Genres.Update(original);
        _logService.Create("Genre with ID {id} successfully updated", Importance.High);
        await _context.SaveChangesAsync();
    }

    public async Task Create(Genre newgenre)
    {
        Genre? genre = await _context.Genres.FirstOrDefaultAsync(g => g.GenreName == newgenre.GenreName);
        if (genre != null)
        {
            _logService.Create("Cannot create genre with name {newgenre.GenreName}, because there is one with the same name", Importance.Low);
            throw new AlreadyExistsException($"genre with {newgenre.GenreName} already exists");
        }
        await _context.Genres.AddAsync(newgenre);
        _logService.Create("Genre with name {newgenre.GenreName} successfully created", Importance.High);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Genre>> GetAll()
    {
        return await _context.Genres
            .AsNoTracking()
            .ToListAsync();
    }
}