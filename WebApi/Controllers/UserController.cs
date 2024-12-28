using System.Security.Claims;
using AutoMapper;
using Data.Dto;
using Data.Models;
using Data.Security;
using Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

public class UserController : ControllerBase
{
    private readonly IUserServices _userServices;
    private readonly IMapper _mapper;

    public UserController(IUserServices userServices, IMapper mapper)
    {
        _userServices = userServices;
        _mapper = mapper;
    }
    
    [HttpPost("[action]")]
    public async Task<ActionResult<UserDto>> Register(UserDto userDto)
    {
        var user = _mapper.Map<User>(userDto);
        await _userServices.Register(user, userDto.Password);
        return Ok();
    }
    
    [HttpPost("[action]")]
    public async Task<ActionResult> Login(UserLoginDto userDto)
    {
        var token = await _userServices.Login(userDto.Username, userDto.Password);
        return Ok(token); 
    }
    
    [HttpGet("[action]")]
    public async Task<ActionResult<UserDto>> GetUser(string username)
    {
        var user = _mapper.Map<UserDto>(_userServices.GetUser(username));
        await _userServices.GetUser(username);
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
    
    [HttpDelete("[action]")]
    public ActionResult DeleteUser(int id)
    {
        _userServices.DeleteUser(id);
        return Ok();
    }
}