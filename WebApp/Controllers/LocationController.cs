using AutoMapper;
using Data.Exceptions;
using Data.Models;
using Data.Services;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers;

public class LocationController : Controller
{
    private readonly ILocationService _locationService;
    private readonly IMapper _mapper;

    public LocationController(ILocationService locationService, IMapper mapper)
    {
        _locationService = locationService;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> DeleteSelectedLocations(List<int> locationIds)
    {
        foreach (var locationId in locationIds)
        {
            await _locationService.Delete(locationId);
        }

        return RedirectToAction("List");
    }

    [HttpGet]
    public async Task<IActionResult> List(string searchString)
    {
        List<Location> locations = await _locationService.GetAll();

        if (!string.IsNullOrEmpty(searchString))
        {
            searchString = searchString.ToLower();
            locations = locations
                .Where(l => l.LocationName.ToLower().Contains(searchString) ||
                            l.Address.ToLower().Contains(searchString))
                .ToList();
        }

        List<LocationViewModel> locationViewModel = _mapper.Map<List<LocationViewModel>>(locations);
        ViewData["CurrentFilter"] = searchString;
        return View(locationViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int IdLocation, string locationName, string address)
    {
        if (ModelState.IsValid)
        {
            var location = await _locationService.Get(IdLocation);
            if (location == null)
            {
                return NotFound();
            }

            location.LocationName = locationName;
            location.Address = address;
            await _locationService.Update(IdLocation,location);
            return RedirectToAction("List");
        }

        return View("List");
    }
    
    public async Task<IActionResult> Details(int locationId)
    {
        var reservations = await _locationService.GetReservationsByLocation(locationId);
        var reservationDetails = _mapper.Map<List<ReservationDetailsViewModel>>(reservations);
        ViewData["LocationId"] = locationId;

        return View("Details", reservationDetails);
    }
}