using AutoMapper;
using Data.Models;
using Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers;

[Authorize]
public class GenreController  : Controller
{
    private readonly IGenreService _genreService;
    private readonly IBookService _bookService;
    private readonly ILocationService _locationService;
    private readonly IMapper _mapper;

    public GenreController(IGenreService genreService, IMapper mapper, IBookService bookService, ILocationService locationService)
    {
        _genreService = genreService;
        _mapper = mapper;
        _bookService = bookService;
        _locationService = locationService;
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
        foreach (var genreId in genreIds)
        {
            var books = await _bookService.GetByGenreId(genreId);
            var bookids = books.Select(b => b.IdBook).ToList();
            await _locationService.RemoveEntryByBookIds(bookids);
            await _bookService.DeleteByGenreId(genreId);
            await _genreService.Delete(genreId);
        }

        if (genreIds.Count == 1)
        {
            TempData["Message"] = "The genre has been deleted.";
        } TempData["Message"] = "The genres have been deleted.";
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
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] GenreViewModel model)
    {
        Genre genre = new Genre
        {
            GenreName = model.GenreName
        };
        await _genreService.Create(genre);
        return Content("Success");    }
}
    
