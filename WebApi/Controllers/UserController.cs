using System.Security.Claims;
using AutoMapper;
using Data.Dto;
using Data.Models;
using Data.Security;
using Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserServices _userServices;
    private readonly IRatingService _ratingService;
    private readonly IMapper _mapper;

    public UserController(IUserServices userServices, IMapper mapper, IRatingService ratingService)
    {
        _userServices = userServices;
        _mapper = mapper;
        _ratingService = ratingService;
    }
    
    [Authorize]
    [HttpGet("[action]")]
    public async Task<ActionResult<UserDto>> GetUser(string username)
    {
        var user = await _userServices.GetUser(username);
        return Ok(user);
    }
    [Authorize]
    [HttpPut("[action]")]
    public async Task<ActionResult<UserDto>> UpdateUser(UserDto userDto)
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        var username = identity.FindFirst(ClaimTypes.Name).Value;
        
        var user = await _userServices.UpdateUser(userDto, username);
        return Ok(user);
    }
    [Authorize]
    [HttpDelete("[action]")]
    public ActionResult DeleteUser(int id)
    {
        _ratingService.DeleteByUserID(id);
        _userServices.DeleteUser(id);
        return Ok();
    }
}