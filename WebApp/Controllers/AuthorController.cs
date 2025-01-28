using AutoMapper;
using Data.Models;
using Data.Services;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers;

public class AuthorController : Controller
{
    private readonly IAuthorService _authorService;
    private readonly IBookService _bookService;
    private readonly ILocationService _locationsService;
    private readonly IMapper _mapper;

    public AuthorController(IAuthorService authorService, IMapper mapper, IBookService bookService, ILocationService locationsService)
    {
        _authorService = authorService;
        _mapper = mapper;
        _bookService = bookService;
        _locationsService = locationsService;
    }
    
    [HttpGet]
    public async Task<IActionResult> List(string searchString)
    {
        List<Author> authors = await _authorService.GetAll();

        if (!string.IsNullOrEmpty(searchString))
        {
            searchString = searchString.ToLower();
            authors = authors.Where(a => a.AuthorName.ToLower().Contains(searchString)).ToList();
        }

        List<AuthorViewModel> authorViewModel = _mapper.Map<List<AuthorViewModel>>(authors);
        ViewData["CurrentFilter"] = searchString;
        return View(authorViewModel);
    }
    
    [HttpPost]
    public async Task<IActionResult> DeleteSelectedAuthors(List<int> authorIds)
    {
        foreach (var authorId in authorIds)
        {
            var books = await _bookService.GetByAuthorId(authorId);
            var bookids = books.Select(b => b.IdBook).ToList();
            await _locationsService.RemoveEntryByBookIds(bookids);
            await _bookService.DeleteByAuthorId(authorId);
            await _authorService.Delete(authorId);
        }
        if (authorIds.Count == 1)
        {
            TempData["Message"] = "The author has been deleted.";
        } TempData["Message"] = "The authors have been deleted.";
        return RedirectToAction("List");
    }
    
    [HttpPost]
    public async Task<IActionResult> Edit(int IdAuthor, string AuthorName)
    {
        if (ModelState.IsValid)
        {
            var author = await _authorService.Get(IdAuthor);
            if (author == null)
            {
                return NotFound();
            }

            author.AuthorName = AuthorName;
            await _authorService.Update(IdAuthor, author);
            return RedirectToAction("List");
        }

        return View("List");
    }
}