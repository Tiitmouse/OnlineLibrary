using System.Security.Claims;
using AutoMapper;
using Data.Dto;
using Data.Models;
using Data.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
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
        ViewBag.SuccessMessage = TempData["SuccessMessage"];
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
            ViewBag.ErrorMessage = ex.Message;
            return View();
        }
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(User user, string password)
    {
        if (string.IsNullOrEmpty(user.Username))
        {
            ViewBag.ErrorMessage = "Username is required.";
            return View();
        }

        if (string.IsNullOrEmpty(password))
        {
            ViewBag.ErrorMessage = "Password is required.";
            return View();
        }

        try
        {
            await _userServices.Register(user, password);
            TempData["SuccessMessage"] = "Registration successful. Please log in.";
            return RedirectToAction("Login", "User");
        }
        catch (Exception ex)
        {
            ViewBag.ErrorMessage = ex.Message;
            return View();
        }
    }
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }
    
    [Authorize]
    public async Task<IActionResult> Details(string username)
    {
        var user = await _userServices.GetUser(username);
        var userDetails = _mapper.Map<UserDetailsViewModel>(user);
        return View(userDetails);
    }
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> EditUserFullName(UserDetailsViewModel model)
    {
        var username = model.Username; 
        await _userServices.UpdateUserFullName(model.FullName, username);
        return Json(new { success = true });
    }
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> EditUserPassword(string oldPassword, string newPassword, string username)
    {
        await _userServices.UpdateUserPassword(oldPassword, newPassword, username);
        return Json(new { success = true });
    }
}