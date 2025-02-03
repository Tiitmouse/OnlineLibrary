using Data.Models;
using Data.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using AutoMapper;
using Data.Dto;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.Models;

namespace WebApp.Controllers;

public class BookController : Controller
{
    private readonly IBookService _bookService;
    private readonly ILocationService _locationService;
    private readonly IAuthorService _authorService;
    private readonly IGenreService _genreService;
    private readonly IRatingService _ratingService;
    private readonly IMapper _mapper;

    public BookController(IBookService bookService, IMapper mapper, ILocationService locationService,
        IAuthorService authorService, IGenreService genreService, IRatingService ratingService)
    {
        _bookService = bookService;
        _mapper = mapper;
        _locationService = locationService;
        _authorService = authorService;
        _genreService = genreService;
        _ratingService = ratingService;
    }

    [HttpGet]
    public async Task<IActionResult> List(string searchString, int pageNumber = 1, int pageSize = 10)
    {
        List<Book> books = await _bookService.GetAll();
        if (!string.IsNullOrEmpty(searchString))
        {
            searchString = searchString.ToLower();
            books = books.Where(b => b.Title.ToLower().Contains(searchString) ||
                                     (b.Author != null && b.Author.AuthorName.ToLower().Contains(searchString)) ||
                                     (b.Genre != null && b.Genre.GenreName.ToLower().Contains(searchString)))
                .ToList();
        }

        int totalBooks = books.Count;
        books = books.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

        List<BookViewModel> bookViewModels = _mapper.Map<List<BookViewModel>>(books);
        ViewData["CurrentFilter"] = searchString;
        ViewData["PageNumber"] = pageNumber;
        ViewData["PageSize"] = pageSize;
        ViewData["TotalBooks"] = totalBooks;

        return View(bookViewModels);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteSelectedBooks(List<int> bookIds)
    {
        foreach (var bookId in bookIds)
        {
            await _locationService.DeleteByBookId(bookId);
            await _bookService.Delete(bookId);
        }

        return RedirectToAction("List");
    }

    public async Task<IActionResult> DeleteByAuthorId(int authorid)
    {
        await _bookService.DeleteByAuthorId(authorid);
        return RedirectToAction("List");
    }

    [HttpPost]
    public async Task<IActionResult> DeleteSingleBook(int IdBook)
    {
        await _locationService.DeleteByBookId(IdBook);
        await _bookService.Delete(IdBook);
        return RedirectToAction("List");
    }

    public IActionResult Details(string returnUrl)
    {
        return View();
    }
    
    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var book = await _bookService.Get(id);
        if (book == null)
        {
            return NotFound();
        }

        var ratings = await _ratingService.GetAll();
        var bookRatings = ratings.Where(r => r.BookId == id).ToList();
        var libraries = await _locationService.GetByBookID(id);
        var averageRating = bookRatings.Any() ? bookRatings.Average(r => r.Rating1.Value) : 0;

        var viewModel = new DetailsBookViewModel
        {
            IdBook = book.IdBook,
            Title = book.Title,
            Description = book.Description,
            PublicationYear = book.PublicationYear,
            Isbn = book.Isbn,
            AuthorName = book.Author.AuthorName,
            GenreName = book.Genre.GenreName,
            AverageRating = averageRating,
            Libraries = libraries.Select(l => new LibraryAvailabilityViewModel
            {
                LocationId = l.LocationId,
                LocationName = l.Location.LocationName,
                LocationAddress = l.Location.Address,
                IsAvailable = !l.Reservations.Any(r => r.BookLocation.BookId == id),
                BookLocationId = l.Id
            }).ToList(),
            Ratings = bookRatings.Select(r => new RatingViewModel
            {
                UserId = r.UserId,
                Rating1 = r.Rating1,
                Comment = r.Comment
            }).ToList()
        };

        return View(viewModel);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var book = await _bookService.Get(id);
        var libraries = await _locationService.GetByBookID(id);
        var genres = await _genreService.GetAll();
        var authors = await _authorService.GetAll();

        DetailsBookViewModel viewModel = new DetailsBookViewModel
        {
            IdBook = book.IdBook,
            Title = book.Title,
            Description = book.Description,
            PublicationYear = book.PublicationYear,
            Isbn = book.Isbn,
            AuthorName = book.Author.AuthorName,
            GenreName = book.Genre.GenreName,
            Libraries = libraries.Select(l => new LibraryAvailabilityViewModel
            {
                LocationId = l.LocationId,
                LocationName = l.Location.LocationName,
                LocationAddress = l.Location.Address,
                IsAvailable = !l.Reservations.Any(r => r.BookLocation.BookId == id),
                BookLocationId = l.Id
            }).ToList(),
            Genres = genres.Select(g => new SelectListItem { Value = g.GenreName, Text = g.GenreName }).ToList(),
            Authors = authors.Select(a => new SelectListItem { Value = a.AuthorName, Text = a.AuthorName }).ToList()
        };
        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> EditAction(int IdBook, BookViewModel newBook)
    {
        var bookEntity = _mapper.Map<Book>(newBook);
        bookEntity.GenreId = (await _genreService.GetByName(newBook.GenreName)).IdGenre;
        bookEntity.AuthorId = (await _authorService.GetByName(newBook.AuthorName)).IdAuthor;

        await _bookService.Update(IdBook, bookEntity);

        return RedirectToAction("List");
    }


    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var genres = await _genreService.GetAll();
        var authors = await _authorService.GetAll();
        var locations = await _locationService.GetAll();

        var newBookViewModel = new NewBookViewModel
        {
            Genres = genres.Select(g => new SelectListItem { Value = g.IdGenre.ToString(), Text = g.GenreName }).ToList(),
            Authors = authors.Select(a => new SelectListItem { Value = a.IdAuthor.ToString(), Text = a.AuthorName }).ToList(),
            Locations = locations.Select(l => new SelectListItem { Value = l.IdLocation.ToString(), Text = l.LocationName }).ToList()
        };
        return View(newBookViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Create(NewBookViewModel newBook)
    {
        var bookEntity = _mapper.Map<Book>(newBook);
        int bookId = await _bookService.Create(bookEntity); 
        await _locationService.AddBookToLocations(bookId, newBook.LocationIds);
        return RedirectToAction("List");
    }
}