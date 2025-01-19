using System.Security.Claims;
using AutoMapper;
using Data.Dto;
using Data.Models;
using Data.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers;

public class UserController : Controller
{
    private readonly IUserServices _userServices;
    private readonly IMapper _mapper;
    private readonly RwaContext _context;

    public UserController(RwaContext context, IUserServices userServices, IMapper mapper)
    {
        _context = context;
        _userServices = userServices;
        _mapper = mapper;
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

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }
    
    public async Task<IActionResult> Details(string username)
    {
        var user = await _userServices.GetUser(username);
        var userDetails = _mapper.Map<UserDetailsViewModel>(user);
        return View(userDetails);
    }
    
    [HttpPost]
    public async Task<IActionResult> EditUserFullName(UserDetailsViewModel model)
    {
        if (ModelState.IsValid)
        {
            var username = model.Username; // Assuming the username is stored in the identity
            await _userServices.UpdateUserFullName(model.FullName, username);

            return Json(new { success = true });
        }

        // Return validation errors
        var errors = ModelState.ToDictionary(
            kvp => kvp.Key,
            kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
        );

        return Json(new { success = false, errors });
    }
    
    [HttpPost]
    public async Task<IActionResult> EditUserPassword(string oldp, string newp, string username) 
    {
        if (ModelState.IsValid)
        { 
            await _userServices.UpdateUserPassword(oldp, newp, username);

            return Json(new { success = true });
        }

        // Return validation errors
        var errors = ModelState.ToDictionary(
            kvp => kvp.Key,
            kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
        );

        return Json(new { success = false, errors });
    }
}