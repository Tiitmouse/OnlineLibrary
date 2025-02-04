using Data.Eunumerators;
using Data.Exceptions;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Services;

public interface ILocationService
{
    public Task<Location> Get(int id);
    public Task<List<BookLocation>> GetByBookID(int id);
    public Task Delete(int id);
    public Task DeleteByBookId(int bookId);
    public Task Update(int id, Location location);
    public Task Create(Location newLocation);
    public Task<List<Location>> GetAll();
    public Task<List<Reservation>> GetReservationsByLocation(int locationId);
    public Task RemoveEntryByBookIds(List<int> bookIds);
    public Task AddBookToLocations(int bookId, List<int> locationIds);
}

public class LocationServices : ILocationService
{
    private readonly RwaContext _context;
    private readonly ILogService _logService;
    
    public LocationServices(RwaContext context, ILogService logService)
    {
        _context = context;
        _logService = logService;
    }
    
    public async Task<Location> Get(int id)
    {
        Location location = await _context.Locations
            .AsNoTracking()
            .FirstOrDefaultAsync(l => l.IdLocation == id);
        if (location == null)
        {
            await _logService.Create($"Failed to fetch location with ID {id}, because there is none with the same ID", Importance.Low);
            throw new NotFoundException($"Location with id {id} not found");
        }
        await _logService.Create($"Location with ID {id} fetched successfully", Importance.Low);
        return location;
    }

    public async Task<List<BookLocation>> GetByBookID(int id)
    {
        return await _context.BookLocations
            .AsNoTracking()
            .Where(l => l.BookId == id) 
            .Include(l => l.Location)
            .Include(b => b.Book)
            .Include(r => r.Reservations)
            .ToListAsync();
    }


    public async Task Delete(int id)
    {
        Location location = await _context.Locations.FirstOrDefaultAsync(l => l.IdLocation == id);
        if (location == null)
        {
            await _logService.Create($"Cannot delete location with ID {id}, because there is none with the same ID", Importance.Low);
            throw new NotFoundException($"location with id {id} not found");
        }
        _context.Locations.Remove(location);
        await _logService.Create($"Location with ID {id} successfully deleted", Importance.High);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteByBookId(int bookId)
    {
        var bookLocations = await _context.BookLocations
            .Where(bl => bl.BookId == bookId)
            .ToListAsync();

        if (!bookLocations.Any())
        {
            await _logService.Create($"No locations found for book with ID {bookId}", Importance.Low);
        }

        _context.BookLocations.RemoveRange(bookLocations);
        await _logService.Create($"All locations for book with ID {bookId} successfully deleted", Importance.High);
        await _context.SaveChangesAsync();
    }

    public async Task Update(int id, Location location)
    {
        Location original = await _context.Locations.FirstOrDefaultAsync(l => l.IdLocation == id);
        if (original == null)
        {
            await _logService.Create($"Cannot update location with ID {id}, because there is none with the same ID", Importance.Low);
            throw new NotFoundException($"Location with id {id} not found");
        }
        
        original.Address = location.Address;
        original.LocationName = location.LocationName;
        
        _context.Locations.Update(original);
        await _logService.Create($"Location with ID {id} successfully updated", Importance.High);
        await _context.SaveChangesAsync();
    }

    public async Task Create(Location newLocation)
    {
        Location? location = _context.Locations.FirstOrDefault(l => l.LocationName == newLocation.LocationName);
        if (location != null)
        {
            await _logService.Create($"Cannot create location with ID {newLocation.IdLocation}, because there is one with the same ID", Importance.Low);
            throw new AlreadyExistsException($"Location with {newLocation.IdLocation} already exists");
        }
        
        await _context.Locations.AddAsync(newLocation);
        await _logService.Create($"Location with ID {newLocation.IdLocation} successfully created", Importance.High);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Location>> GetAll()
    {
        return await _context.Locations
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<Reservation>> GetReservationsByLocation(int locationId)
    {
        return await _context.Reservations
            .Include(r => r.BookLocation)
            .ThenInclude(bl => bl.Book)
            .Include(r => r.User)
            .Where(r => r.BookLocation.LocationId == locationId)
            .ToListAsync();
    }

    public async Task RemoveEntryByBookIds(List<int> bookIds)
    {
        _context.BookLocations.RemoveRange(_context.BookLocations.Where(bl => bookIds.Contains(bl.BookId)));
        await _context.SaveChangesAsync();
    }
    
    
    public async Task AddBookToLocations(int bookId, List<int> locationIds)
    {
        var list = new List<BookLocation>();
        
        foreach (var locationId in locationIds)
        {
            list.Add(new BookLocation
            {
                BookId = bookId,
                LocationId = locationId
            });
        }

        await _context.BookLocations.AddRangeAsync(list);
        await _context.SaveChangesAsync();
    }

}