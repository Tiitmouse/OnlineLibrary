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

    public BookController(IBookService bookService)
    {
        _bookService = bookService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> FetchBook(int id)
    {
        Book book = await _bookService.Get(id);
        return Ok(book);
    }
    [HttpDelete("{id}")]
    public IActionResult DeleteBook(int id)
    {
        return NoContent();
    } 
    [HttpPost]
    public IActionResult CreateBook(int id)
    {
        return Ok($"book {id} added");
    } 
    [HttpPut]
    public IActionResult EditBook(int id)
    {
        return Ok($"book {id} edited");
    } 
}