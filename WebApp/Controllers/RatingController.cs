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
    public async Task<IActionResult> AddRating(RatingViewModel ratingViewModel)
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
}