using Data.Eunumerators;
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
            _logService.Create("Failed to fetch location with ID {id}, because there is none with the same ID", Importance.Low);
            throw new NotFoundException($"Location with id {id} not found");
        }
        _logService.Create("Location with ID {id} fetched successfully", Importance.Low);
        return location;
    }

    public async Task Delete(int id)
    {
        Location location = await _context.Locations.FirstOrDefaultAsync(l => l.IdLocation == id);
        if (location == null)
        {
            _logService.Create("Cannot delete location with ID {id}, because there is none with the same ID", Importance.Low);
            throw new NotFoundException($"location with id {id} not found");
        }
        _context.Locations.Remove(location);
        _logService.Create("Location with ID {id} successfully deleted", Importance.High);
        await _context.SaveChangesAsync();
    }

    public async Task Update(int id, Location location)
    {
        Location original = await _context.Locations.FirstOrDefaultAsync(l => l.IdLocation == id);
        if (original == null)
        {
            _logService.Create("Cannot update location with ID {id}, because there is none with the same ID", Importance.Low);
            throw new NotFoundException($"Location with id {id} not found");
        }
        
        original.Address = location.Address;
        original.LocationName = location.LocationName;
        
        _context.Locations.Update(original);
        _logService.Create("Location with ID {id} successfully updated", Importance.High);
        await _context.SaveChangesAsync();
    }

    public async Task Create(Location newLocation)
    {
        Location? location = _context.Locations.FirstOrDefault(l => l.LocationName == newLocation.LocationName);
        if (location != null)
        {
            _logService.Create("Cannot create location with ID {id}, because there is one with the same ID", Importance.Low);
            throw new AlreadyExistsException($"Location with {newLocation.IdLocation} already exists");
        }
        
        await _context.Locations.AddAsync(newLocation);
        _logService.Create("Location with ID {id} successfully created", Importance.High);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Location>> GetAll()
    {
        return await _context.Locations
            .AsNoTracking()
            .ToListAsync();
    }
}