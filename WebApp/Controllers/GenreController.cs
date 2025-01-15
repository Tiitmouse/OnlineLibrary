using AutoMapper;
using Data.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

public class GenreController  : Controller
{
    private readonly IGenreService _genreService;
    private readonly IMapper _mapper;

    public GenreController(IGenreService genreService, IMapper mapper)
    {
        _genreService = genreService;
        _mapper = mapper;
    }
    
    public IActionResult List(string returnUrl)
    {
        return View();
    }
    
}