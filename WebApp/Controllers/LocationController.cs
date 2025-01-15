using AutoMapper;
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

    public IActionResult List(string returnUrl)
    {
        return View();
    }
}
    
    
    // public async Task<IActionResult> List(string searchString)
    // {
    //     List<Location> locations = await _locationService.GetAll();
    //
    //     if (!string.IsNullOrEmpty(searchString))
    //     {
    //         searchString = searchString.ToLower();
    //         locations = locations.Where(l => l.LocationName.ToLower().Contains(searchString)).ToList();
    //     }
    //
    //     List<LocationViewModel> authorViewModel = _mapper.Map<List<LocationViewModel>>(locations);
    //     ViewData["CurrentFilter"] = searchString;
    //     return View(LocationViewModel);
    // }
