using AutoMapper;
using Data.Dto;
using Data.Models;
using Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Authorize]
[Route("api/[controller]")]
public class BookController : ControllerBase
{
    private readonly IBookService _bookService;
    private readonly ILocationService _locationService;
    private readonly IMapper _mapper;

    public BookController(IBookService bookService, IMapper mapper, ILocationService locationService)
    {
        _bookService = bookService;
        _mapper = mapper;
        _locationService = locationService;
    }

    [HttpGet("[action]/{id}")]
    public async Task<IActionResult> FetchBook(int id)
    {
        var book = await _bookService.Get(id);
        var bookDetailsDto = _mapper.Map<BookDetailsDto>(book);
        return Ok(bookDetailsDto);
    }
    [HttpDelete("([action]/{id}")]
    public async Task<IActionResult> DeleteBook(int id)
    {
        await _bookService.Delete(id);
        return NoContent();
    } 
    [HttpPost("[action]")]
    public async Task<IActionResult> CreateBook(NewBookDto bookDto)
    {
        var newBook = _mapper.Map<Book>(bookDto);
        int bookId = await _bookService.Create(newBook);
        await _locationService.AddBookToLocations(bookId, bookDto.LocationsId);
        return Ok($"book with {newBook.Isbn} isbn has been added");
    } 
    [HttpPut("[action]")]
    public async Task<IActionResult> EditBook(int id, Book book)
    {
        await _bookService.Update(id, book);
        return Ok($"book {id} edited");
    }
    [HttpGet("[action]")]
    public async Task<IActionResult> FetchBooks()
    {
        var books = await _bookService.GetAll();
        var bookDtos = _mapper.Map<List<BookDto>>(books);
        return Ok(bookDtos);
    }
    [HttpGet("[action]")]
    public async Task<IActionResult> Search([FromQuery]string seachTerm, [FromQuery]int n, [FromQuery]int page)
    {
        var books = await _bookService.SearchAndPaginate(seachTerm, n, page);
        var bookDtos = _mapper.Map<List<BookDto>>(books);
        return Ok(bookDtos);
    }
    
}