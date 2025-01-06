using Data.Models;
using Data.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using AutoMapper;
using Data.Dto;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookService _bookService;
        private readonly IMapper _mapper;

        public BookController(IBookService bookService, IMapper mapper)
        {
            _bookService = bookService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> List(string searchString)
        {
            List<Book> books = await _bookService.GetAll();
            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                books = books.Where(b => b.Title.ToLower().Contains(searchString) ||
                                         (b.Author != null && b.Author.AuthorName.ToLower().Contains(searchString)) ||
                                         (b.Genre != null && b.Genre.GenreName.ToLower().Contains(searchString))).ToList();
            }
            List<BookViewModel> bookViewModels = _mapper.Map<List<BookViewModel>>(books);
            ViewData["CurrentFilter"] = searchString;
            return View(bookViewModels);
        }
        
        [HttpPost]
        public async Task<IActionResult> DeleteSelectedBooks(List<int> bookIds)
        {
            foreach (var bookId in bookIds)
            {
                await _bookService.Delete(bookId);
            }
            return RedirectToAction("List");
        }
    }
}