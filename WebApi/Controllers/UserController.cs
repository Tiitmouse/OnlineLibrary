using Data.Dto;
using Data.Models;
using Data.Security;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

public class UserController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly RwaContext _context;

    public UserController(IConfiguration configuration, RwaContext context)
    {
        _configuration = configuration;
        _context = context;
    }
    
    [HttpGet("[action]")]
    public ActionResult GetToken()
    {
        try
        {
            var secureKey = _configuration["JWT:SecureKey"];
            var serializedToken = JwtTokenProvider.CreateToken(secureKey, 10);

            return Ok(serializedToken);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
    
    [HttpPost("[action]")]
    public ActionResult<UserDto> Register(UserDto userDto)
    {
        try
        {
            var trimmedUsername = userDto.Username.Trim();
            if (_context.Users.Any(x => x.Username.Equals(trimmedUsername)))
                return BadRequest($"Username {trimmedUsername} already exists");

            var b64hash = PasswordHashProvider.GetHash(userDto.Password);

            var user = new User
            {
                Username = userDto.Username,
                FullName = userDto.FullName,
                PasswordHash = b64hash,
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok(userDto);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
    
    [HttpPost("[action]")]
    public ActionResult Login(UserLoginDto userDto)
    {
        try
        {
            var genericLoginFail = "Incorrect username or password";

            // Try to get a user from database
            var existingUser = _context.Users.FirstOrDefault(x => x.Username == userDto.Username);
            if (existingUser == null)
                return BadRequest(genericLoginFail);

            // Check is password hash matches
            var b64hash = PasswordHashProvider.GetHash(userDto.Password);
            if (b64hash != existingUser.PasswordHash)
                return BadRequest(genericLoginFail);

            // Create and return JWT token
            var secureKey = _configuration["JWT:SecureKey"];
            var serializedToken = 
                JwtTokenProvider.CreateToken(
                    secureKey, 
                    120, 
                    userDto.Username); // Hardcoded example!

            return Ok(serializedToken);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}