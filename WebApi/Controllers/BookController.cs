using AutoMapper;
using Data.Dto;
using Data.Models;
using Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;
//TODO uncomment after implementing jwt auth
//[Authorize]
[Route("api/[controller]")]
public class BookController : ControllerBase
{
    private readonly IBookService _bookService;
    private readonly IMapper _mapper;

    public BookController(IBookService bookService, IMapper mapper)
    {
        _bookService = bookService;
        _mapper = mapper;
    }

    [HttpGet("[action]/{id}")]
    public async Task<IActionResult> FetchBook(int id)
    {
        Book book = await _bookService.Get(id);
        return Ok(book);
    }
    [HttpDelete("([action]/{id}")]
    public IActionResult DeleteBook(int id)
    {
        _bookService.Delete(id);
        return NoContent();
    } 
    [HttpPost("[action]")]
    public IActionResult CreateBook(NewBookDto bookDto)
    {
        Book newBook = _mapper.Map<Book>(bookDto);
        _bookService.Create(newBook);
        return Ok($"book with {newBook.Isbn} isbn has been added");
    } 
    [HttpPut("[action]")]
    public IActionResult EditBook(int id, Book book)
    {
        _bookService.Update(id, book);
        return Ok($"book {id} edited");
    } 
    [HttpGet("[action]")]
    public async Task<IActionResult> FetchBooks()
    {
        return Ok(await _bookService.GetAll());
    }
    
}