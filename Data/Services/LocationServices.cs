using Data.Exceptions;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Services;

public interface ILocationService
{
    public Task<Location> Get(int id);
    public Task Delete(int id);
    public Task Update(int id, Location location);
    public Task Create(Location newLocation);
    public Task<List<Location>> GetAll();
}

public class LocationServices : ILocationService
{
    private readonly RwaContext _context;
    
    public LocationServices(RwaContext context)
    {
        _context = context;
    }
    
    public async Task<Location> Get(int id)
    {
        Location location = await _context.Locations
            .AsNoTracking()
            .FirstOrDefaultAsync(l => l.IdLocation == id);
        if (location == null)
        {
            throw new NotFoundException($"Location with id {id} not found");
        }
        return location;
    }

    public async Task Delete(int id)
    {
        Location location = await _context.Locations.FirstOrDefaultAsync(l => l.IdLocation == id);
        _context.Locations.Remove(location);
        await _context.SaveChangesAsync();
    }

    public async Task Update(int id, Location location)
    {
        Location original = await _context.Locations.FirstOrDefaultAsync(l => l.IdLocation == id);
        if (original == null)
        {
            throw new NotFoundException($"Location with id {id} not found");
        }
        
        original.Address = location.Address;
        original.LocationName = location.LocationName;
        
        _context.Locations.Update(original);
        await _context.SaveChangesAsync();
    }

    public async Task Create(Location newLocation)
    {
        Location? location = _context.Locations.FirstOrDefault(l => l.LocationName == newLocation.LocationName);
        if (location != null)
        {
            throw new AlreadyExistsException($"Location with {newLocation.IdLocation} already exists");
        }
        
        await _context.Locations.AddAsync(newLocation);
        await _context.SaveChangesAsync();

    }

    public async Task<List<Location>> GetAll()
    {
        return await _context.Locations
            .AsNoTracking()
            .ToListAsync();
    }
}