using System.Security.Claims;
using AutoMapper;
using Data.Dto;
using Data.Models;
using Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;
[Route("api/[controller]")]
public class AuthorisationController : ControllerBase
{
    private readonly IUserServices _userServices;
    private readonly IMapper _mapper;

    public AuthorisationController(IUserServices userServices, IMapper mapper)
    {
        _userServices = userServices;
        _mapper = mapper;
    }

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
}