using AutoMapper;
using Data.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

public class AuthorController : Controller
{
    private readonly IAuthorService _authorService;
    private readonly IMapper _mapper;

    public AuthorController(IAuthorService authorService, IMapper mapper)
    {
        _authorService = authorService;
        _mapper = mapper;
    }
    
    public IActionResult List(string returnUrl)
    {
        return View();
    }
}