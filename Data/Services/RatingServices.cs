using Data.Eunumerators;
using Data.Exceptions;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Services;

public interface IRatingService
{
    public Task<Rating> Get(int id);
    public Task Delete(int id);
    public Task DeleteByBookID(int bookId);
    
    public Task DeleteByUserID(int userId);
    public Task Create(Rating newRating);
    public Task<List<Rating>> GetAll();
}

public class RatingServices  : IRatingService
{
    private readonly RwaContext _context;
    private readonly ILogService _logService;

    public RatingServices(RwaContext context, ILogService logService)
    {
        _context = context;
        _logService = logService;
    }


    public async Task<Rating> Get(int id)
    {
        Rating rating = await _context.Ratings
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.IdRating == id);
        if (rating == null)
        {
            await _logService.Create($"Failed to fetch rating with ID {id}, because there is none with the same ID", Importance.Low);
            throw new NotFoundException($"Rating with id {id} not found");
        }
        await _logService.Create($"Rating with ID {id} fetched successfully", Importance.Low);
        return rating;
    }

    public async Task Delete(int id)
    {
        Rating rating = await _context.Ratings.FirstOrDefaultAsync(a => a.IdRating == id);
        if (rating == null)
        {
            await _logService.Create($"Cannot delete rating with ID {id}, because there is none with the same ID", Importance.Low);
            throw new NotFoundException($"Rating with id {id} not found");
        }
        _context.Ratings.Remove(rating);
        await _logService.Create($"Rating with ID {id} successfully deleted", Importance.High);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteByBookID(int bookId)
    {
        var ratings = await _context.Ratings.Where(r => r.BookId == bookId).ToListAsync();
        if (ratings == null || !ratings.Any())
        {
            await _logService.Create($"No ratings found for book with ID {bookId}", Importance.Low);
            return;
        }
        _context.Ratings.RemoveRange(ratings);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteByUserID(int userId)
    {
        var ratings = await _context.Ratings.Where(r => r.UserId == userId).ToListAsync();
        if (ratings == null || !ratings.Any())
        {
            await _logService.Create($"No ratings found for user with ID {userId}", Importance.Low);
            return;
        }
        _context.Ratings.RemoveRange(ratings);
        await _context.SaveChangesAsync();
    }

    public async Task Create(Rating newRating)
    {
        Rating? rating = await _context.Ratings.FirstOrDefaultAsync(r => r.IdRating == newRating.IdRating);
        if (rating != null)
        {
            if (newRating.UserId == rating.UserId)
            {
                await _logService.Create($"Failed to add new rating, because rating with user ID {newRating.UserId} already existst", Importance.Low);
                throw new AlreadyExistsException($"Rating from user with id {newRating.UserId} already exists");
            }
            if (newRating.BookId == rating.BookId)
            {
                await _logService.Create(
                    $"Failed to add new rating, because rating with book ID {newRating.BookId} already existst",
                    Importance.Low);
                throw new AlreadyExistsException($"Rating for book with id {newRating.BookId} already exists");
            }
        }

        await _context.Ratings.AddAsync(newRating);
        await _logService.Create($"Rating with ID {newRating.IdRating} successfully added", Importance.Low);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Rating>> GetAll()
    {
        return await _context.Ratings
            .AsNoTracking()
            .Include(r => r.User )
            .OrderByDescending(r => r.Rating1)
            .ToListAsync();
    }
}