using AutoMapper;
using Data.Dto;
using Data.Models;
using Data.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

public class LocationController : ControllerBase
{
    private readonly ILocationService _locationService;
    private readonly IMapper _mapper;
    
    public LocationController(ILocationService locationService, IMapper mapper)
    {
        _locationService = locationService;
        _mapper = mapper;
    }
    
    [HttpGet("[action]/{id}")]
    public async Task<IActionResult> FetchLocation(int id)
    {
        var location = await _locationService.Get(id);
        return Ok(location);
    }
    
    [HttpDelete("[action]/{id}")]
    public async Task<IActionResult> DeleteLocation(int id)
    {
        await _locationService.Delete(id);
        return NoContent();
    }
    
    [HttpPut("[action]")]
    public async Task<IActionResult> EditLocation(int id, Location location)
    {
        await _locationService.Update(id, location);
        return Ok($"location {id} edited");
    }
    
    [HttpPost("[action]")]
    public async Task<IActionResult> CreateLocation(LocationDto dto)
    {
        var newLocation = _mapper.Map<Location>(dto);
        await _locationService.Create(newLocation);
        return Ok($"location {newLocation.LocationName} created");
    }
    
    [HttpGet("[action]")]
    public async Task<IActionResult> FetchLocations()
    {
        var locations = await _locationService.GetAll();
        return Ok(locations);
    }
    
}