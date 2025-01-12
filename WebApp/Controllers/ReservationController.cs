using AutoMapper;
using Data.Models;
using Data.Services;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers;

public class ReservationController : Controller
{
    private readonly IReservationService _reservationService;
    private readonly IMapper _mapper;

    public ReservationController(IReservationService reservationService, IMapper mapper)
    {
        _reservationService = reservationService;
        _mapper = mapper;
    }
   
    [HttpGet]
    public async Task<IActionResult> ReservationStatusByBookID(int bookId)
    {
        Reservation reservation = await _reservationService.GetByBookId(bookId);
        ReservationViewModel reservationViewModel = _mapper.Map<ReservationViewModel>(reservation);
        
        return View(reservationViewModel);
    }
    
}