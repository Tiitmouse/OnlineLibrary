using AutoMapper;
using Data.Models;
using Data.Services;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers;

public class RatingController : Controller
{
    private readonly IRatingService _ratingService;
    private readonly IMapper _mapper;


    public RatingController(IRatingService ratingService, IMapper mapper)
    {
        _ratingService = ratingService;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> AddRating([FromBody]RatingViewModel ratingViewModel)
    {
        Rating rating = new Rating
        {
            UserId = ratingViewModel.UserId,
            BookId = ratingViewModel.BookId,
            Rating1 = ratingViewModel.Rating,
            Comment = ratingViewModel.Comment
        };
        await _ratingService.Create(rating);
        return Content("Success");
    }
    
    [HttpGet] 
    public async Task<IActionResult> GetRating(int userId, int bookId)
    {
        var rating = await _ratingService.GetRatingByUserAndBookAsync(userId, bookId);
        if (rating == null)
        {
            return NotFound("Rating not found.");
        }

        var ratingViewModel = _mapper.Map<RatingViewModel>(rating);
        return Ok(ratingViewModel);
    }
}