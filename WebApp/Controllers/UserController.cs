using System.Security.Claims;
using Data.Dto;
using Data.Models;
using Data.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

public class UserController : Controller
{
    private readonly IUserServices _userServices;
    private readonly RwaContext _context;

    public UserController(RwaContext context, IUserServices userServices)
    {
        _context = context;
        _userServices = userServices;
    }
    
    public IActionResult Login(string returnUrl)
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Login([FromForm] UserLoginDto dto)
    {
        try
        {
            var claimsIdentity = await _userServices.LoginCookie(dto.Username, dto.Password);
            var authProperties = new AuthenticationProperties();

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return RedirectToAction("Index", "Home");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    public IActionResult Register()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Register(User user, string password)
    {
        try
        {
            await _userServices.Register(user, password);
            return Ok("User registered");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}