using AutoMapper;
using Data.Models;
using Data.Services;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers;

public class ReservationController : Controller
{
    private readonly IReservationService _reservationService;
    private readonly IUserServices _userServices;
    private readonly IMapper _mapper;

    public ReservationController(IReservationService reservationService, IMapper mapper, IUserServices userServices)
    {
        _reservationService = reservationService;
        _mapper = mapper;
        _userServices = userServices;
    }
   
    [HttpGet]
    public async Task<IActionResult> ReservationStatusByBookID(int bookId)
    {
        Reservation reservation = await _reservationService.GetByBookId(bookId);
        ReservationViewModel reservationViewModel = _mapper.Map<ReservationViewModel>(reservation);
        
        return View(reservationViewModel);
    }
    
    [HttpPost]
    public async Task<IActionResult> ReserveBook(ReservationViewModel reservationViewModel)
    {
        Reservation reservation = _mapper.Map<Reservation>(reservationViewModel);
        int bookId = await _reservationService.Reserve(reservation);

        TempData["Message"] = "The book has been reserved.";
        return RedirectToAction("Details", "Book", new { id = bookId });
    }
    
    [HttpPost]
    public async Task<IActionResult> CancelReservation(int reservationId, int locationId)
    {
        await _reservationService.Cancel(reservationId);

        TempData["Message"] = "The reservation has been canceled.";
        return RedirectToAction("Details", "Location", new { locationId = locationId });
    }
    
    [HttpGet]
    public async Task<IActionResult> UserReservations(string username)
    {
        User user = await _userServices.GetUser(username);
        var reservations = await _reservationService.GetByUserId(user.IdUser);
        var userReservations = _mapper.Map<List<UserReservation>>(reservations);

        return View("UserReservations", userReservations);
    }
}