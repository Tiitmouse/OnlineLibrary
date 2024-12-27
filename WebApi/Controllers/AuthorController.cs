using AutoMapper;
using Data.Dto;
using Data.Models;
using Data.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

public class AuthorController : ControllerBase
{
    private readonly IAuthorService _authorService;
    private readonly IMapper _mapper;
    
    public AuthorController(IAuthorService authorService, IMapper mapper)
    {
        _authorService = authorService;
        _mapper = mapper;
    }
    
    [HttpGet("[action]/{id}")]
    public async Task<IActionResult> FetchAuthor(int id)
    {
        var author = await _authorService.Get(id);
        var authorDto = _mapper.Map<AuthorDto>(author);
        return Ok(authorDto);
    }
    
    [HttpDelete("[action]/{id}")]
    public async Task<IActionResult> DeleteAuthor(int id)
    {
        await _authorService.Delete(id);
        return NoContent();
    }
    
    [HttpPost("[action]")]
    public async Task<IActionResult> CreateAuthor(AuthorDto dto)
    {
        var author = _mapper.Map<Author>(dto);
        await _authorService.Create(author);
        return Ok($"author {author.AuthorName} has been added");
    }
    
    [HttpPut("[action]")]
    public async Task<IActionResult> EditAuthor(int id, Author author)
    {
        await _authorService.Update(id, author);
        return Ok($"author {id} edited");
    }
    
    [HttpGet("[action]")]
    public async Task<IActionResult> FetchAuthors()
    {
        var authors = await _authorService.GetAll();
        var authorDtos = _mapper.Map<List<AuthorDto>>(authors);
        return Ok(authorDtos);
    }
    
}