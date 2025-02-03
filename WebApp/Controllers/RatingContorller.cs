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
    
    // [HttpGet]
    // public async Task<IActionResult> BookRatings(int bookId)
    // {
    //     var ratings = await _ratingService.GetAll();
    //     var bookRatings = ratings.Where(r => r.BookId == bookId).ToList();
    //     var averageRating = bookRatings.Any() ? bookRatings.Average(r => r.Rating1.Value) : 0;
    //
    //     var viewModel = new RatingDetailViewModel
    //     {
    //         AverageRating = averageRating,
    //         Ratings = bookRatings.Select(r => new RatingViewModel
    //         {
    //             UserId = r.UserId,
    //             Rating1 = r.Rating1,
    //             Comment = r.Comment
    //         }).ToList()
    //     };
    //
    //     return View(viewModel);
    // }

    [HttpPost]
    public async Task<IActionResult> AddRating(RatingViewModel ratingViewModel)
    {
        Rating rating = new Rating
        {
            UserId = ratingViewModel.UserId,
            BookId = ratingViewModel.BookId,
            Rating1 = ratingViewModel.Rating1
        };
        await _ratingService.Create(rating);
        return Content("Success");
    }
    
    [HttpGet]
    public async Task<double> Calculate(int bookId)
    {
        var ratings = await _ratingService.GetAll();
        var bookRatings = ratings.Where(r => r.BookId == bookId).Select(r => r.Rating1.Value);
        return bookRatings.Any() ? bookRatings.Average() : 0;
    }
}