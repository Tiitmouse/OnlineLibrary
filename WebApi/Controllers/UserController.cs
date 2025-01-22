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
    private readonly IMapper _mapper;

    public UserController(IUserServices userServices, IMapper mapper)
    {
        _userServices = userServices;
        _mapper = mapper;
    }
    //TODO: separate register and login and changepassword to auth controller  don't forget to change it in the frontend js :)
    [HttpPost("[action]")]
    public async Task<ActionResult<UserDto>> Register([FromBody]UserDto userDto)
    {
        var user = _mapper.Map<User>(userDto);
        await _userServices.Register(user, userDto.Password);
        return Ok();
    }
    
    [HttpPost("[action]")]
    public async Task<ActionResult> Login([FromBody]UserLoginDto userDto)
    {
        var token = await _userServices.Login(userDto.Username, userDto.Password);
        return Ok(token); 
    }
    //TODO: make endpoint changepassword
    
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
        _userServices.DeleteUser(id);
        return Ok();
    }
}