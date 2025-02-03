using AutoMapper;
using Data.Models;
using Data.Services;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers;

public class RatingContorller : Controller
{
    private readonly IRatingService _ratingService;
    private readonly IMapper _mapper;


    public RatingContorller(IRatingService ratingService, IMapper mapper)
    {
        _ratingService = ratingService;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> AddRating(RatingViewModel ratingViewModel)
    {
        var rating = _mapper.Map<Rating>(ratingViewModel);
        await _ratingService.Create(rating);

        return RedirectToAction("Details", "Book", new { id = ratingViewModel.BookId });
    }
}