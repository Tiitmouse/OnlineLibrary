using AutoMapper;
using Data.Dto;
using Data.Models;
using Data.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

public class ReservationController : ControllerBase
{
    private readonly IReservationService _reservationService;
    private readonly IMapper _mapper;

    public ReservationController(IReservationService reservationService, IMapper mapper)
    {
        _reservationService = reservationService;
        _mapper = mapper;
    }
    
    [HttpGet("[action]/{id}")]
    public async Task<IActionResult> FetchReservation(int id)
    {
        var reservation = await _reservationService.Get(id);
        var reservationDto = _mapper.Map<ReservationDto>(reservation);
        return Ok(reservationDto);
    }
    [HttpDelete("[action]/{id}")]
    public async Task<IActionResult> CancelReservation(int id)
    {
        await _reservationService.Cancel(id);
        return NoContent();
    }
    [HttpPut("[action]/{id}")]
    public async Task<IActionResult> ChangeReservationStatus(int id, bool newStatus)
    {
        if (newStatus == null)
        {
            return BadRequest("New status cannot be null");
        }
        await _reservationService.ChangeStatus(id, newStatus);
        return Ok($"Reservation {id} status changed to {newStatus}");
    }
    [HttpPost("[action]")]
    public async Task<IActionResult> CreateReservation(ReservationDto reservationDto)
    {
        var newReservation = _mapper.Map<Reservation>(reservationDto);
        await _reservationService.Reserve(newReservation);
        return Ok($"Reservation with {newReservation.IdReservation} id has been added");
    }
    [HttpGet("[action]")]
    public async Task<IActionResult> FetchReservations()
    {
        var reservations = await _reservationService.GetAll();
        var reservationDtos = _mapper.Map<List<ReservationDto>>(reservations);
        return Ok(reservationDtos);
    }
}