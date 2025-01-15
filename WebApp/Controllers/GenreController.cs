using AutoMapper;
using Data.Models;
using Data.Services;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers;

public class GenreController  : Controller
{
    private readonly IGenreService _genreService;
    private readonly IBookService _bookService;
    private readonly IMapper _mapper;

    public GenreController(IGenreService genreService, IMapper mapper, IBookService bookService)
    {
        _genreService = genreService;
        _mapper = mapper;
        _bookService = bookService;
    }
    
    [HttpGet]
    public async Task<IActionResult> List(string searchString)
    {
        List<Genre> genres = await _genreService.GetAll();

        if (!string.IsNullOrEmpty(searchString))
        {
            searchString = searchString.ToLower();
            genres = genres.Where(g => g.GenreName.ToLower().Contains(searchString)).ToList();
        }

        List<GenreViewModel> genreViewModel = _mapper.Map<List<GenreViewModel>>(genres);
        ViewData["CurrentFilter"] = searchString;
        return View(genreViewModel);
    }
    
    [HttpPost]
    public async Task<IActionResult> DeleteSelectedGenres(List<int> genreIds)
    {
        foreach (var authorId in genreIds)
        {
            await _bookService.DeleteByAuthorId(authorId);
            await _genreService.Delete(authorId);
        }
        return RedirectToAction("List");
    }
    
    [HttpPost]
    public async Task<IActionResult> Edit(int IdGenre, string GenreName)
    {
        if (ModelState.IsValid)
        {
            var genre = await _genreService.Get(IdGenre);
            if (genre == null)
            {
                return NotFound();
            }

            genre.GenreName = GenreName;
            await _genreService.Update(IdGenre, genre);
            return RedirectToAction("List");
        }

        return View("List");
    }
    
}