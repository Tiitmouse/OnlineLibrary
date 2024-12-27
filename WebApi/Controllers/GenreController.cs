using AutoMapper;
using Data.Dto;
using Data.Models;
using Data.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

public class GenreController : ControllerBase
{
    private readonly IGenreService _genreService;
    public GenreController(IGenreService genreService)
    {
        _genreService = genreService;
    }
    
    [HttpGet("[action]/{id}")]
    public async Task<IActionResult> FetchGenre(int id)
    {
        var genre = await _genreService.Get(id);
        return Ok(genre);
    }
    
    [HttpDelete("[action]/{id}")]
    public async Task<IActionResult> DeleteGenre(int id)
    {
        await _genreService.Delete(id);
        return NoContent();
    }
    
    [HttpPost("[action]")]
    public async Task<IActionResult> CreateGenre(Genre genre)
    {
        await _genreService.Create(genre);
        return Ok($"Genre {genre.GenreName} has been added");
    }
    
    [HttpPut("[action]")]
    public async Task<IActionResult> EditGenre(int id, Genre genre)
    {
        await _genreService.Update(id, genre);
        return Ok($"Genre {id} edited");
    }
    
    [HttpGet("[action]")]
    public async Task<IActionResult> FetchGenres()
    {
        var genres = await _genreService.GetAll();
        return Ok(genres);
    }
}