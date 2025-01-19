using System.Security.Claims;
using Data.Dto;
using Data.Eunumerators;
using Data.Exceptions;
using Data.Models;
using Data.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Data.Services;

public interface IUserServices
{
    Task<User> Register(User user, string password);
    Task<string> Login(string username, string password);
    Task<User> GetUser(string username);
    Task<User> UpdateUser(UserDto userDto, string username);
    Task<User> UpdateUserFullName(string fullname, string username);
    Task<User> UpdateUserPassword(string oldPassword,string newPassword, string username);
    Task<ClaimsIdentity> LoginCookie(string username, string password);
    Task DeleteUser(int id);
}

public class UserServices : IUserServices
{
    private readonly IConfiguration _configuration;
    private readonly RwaContext _context;
    private readonly ILogService _logService;

    public UserServices(IConfiguration configuration, RwaContext context, ILogService logService)
    {
        _configuration = configuration;
        _context = context;
        _logService = logService;
    }

    public async Task<User> Register(User user, string password)
    {
        var trimmedUsername = user.Username.Trim();
        if (await _context.Users.AnyAsync(x => x.Username.Equals(trimmedUsername)))
        { 
            await _logService.Create("User failed to register", Importance.Low);
            throw new Exception($"Username {trimmedUsername} already exists");
        }
        
        var b64hash = PasswordHashProvider.GetHash(password);
        user.PasswordHash = b64hash;

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        await _logService.Create($"User {user.Username} successfully registered", Importance.Low);
        return user;
    }

    public async Task<string> Login(string username, string password)
    {
        var genericLoginFail = "Incorrect username or password";

        var existingUser = await _context.Users.FirstOrDefaultAsync(x => x.Username == username);
        if (existingUser == null)
        { 
            await _logService.Create("User failed to log in", Importance.Medium);
            throw new Exception(genericLoginFail);
        }

        var b64hash = PasswordHashProvider.GetHash(password);
        if (b64hash != existingUser.PasswordHash)
        { 
            await _logService.Create("User failed to log in", Importance.Medium);
            throw new Exception(genericLoginFail);
        }


        var secureKey = _configuration["JWT:SecureKey"];
        await _logService.Create($"User {username} successfully logged in", Importance.Low);
        return JwtTokenProvider.CreateToken(secureKey, 120, username);
    }

    public async Task<ClaimsIdentity> LoginCookie(string username, string password)
    {
        var genericLoginFail = "Incorrect username or password";

        var existingUser = await _context.Users.FirstOrDefaultAsync(x => x.Username == username);
        if (existingUser == null)
        { 
            await _logService.Create("User failed to log in", Importance.Medium);
            throw new Exception(genericLoginFail);
        }

        var b64hash = PasswordHashProvider.GetHash(password);
        if (b64hash != existingUser.PasswordHash)
        { 
            await _logService.Create("User failed to log in", Importance.Medium);
            throw new Exception(genericLoginFail);
        }

        var claims = new List<Claim>() {
            new Claim(ClaimTypes.Name, username),
            new Claim("admin", existingUser.IsAdmin.ToString()),
            new Claim("id", existingUser.IdUser.ToString())
        };

        var claimsIdentity = new ClaimsIdentity(
            claims,
            CookieAuthenticationDefaults.AuthenticationScheme);

        var authProperties = new AuthenticationProperties();
        await _logService.Create($"User {username} successfully logged in", Importance.Low);
        return claimsIdentity;
    }

    public async Task<User> GetUser(string username)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == username);
        if (user == null)
        {
            await _logService.Create("Failed to fetch user", Importance.Low);
            throw new NotFoundException("User not found");
        }

        await _logService.Create("User fetched successfully", Importance.Low);
        return user;
    }
    
    public async Task<User> UpdateUserFullName(string fullname, string username)
    {
        var user = _context.Users.FirstOrDefault(u => u.Username == username);
        if (user == null)
        {
            await _logService.Create($"Failed to update user {username}, because there is none with the same username", Importance.Low);
            throw new NotFoundException("User not found");
        }
        user.FullName = fullname;
        
        await _context.SaveChangesAsync();
        await _logService.Create($"User {username} successfully updated", Importance.Medium);
        return user;
    }
    
    public async Task<User> UpdateUserPassword(string oldPassword, string newPassword, string username)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (user == null)
        {
            await _logService.Create($"Failed to update password for user {username}, because there is none with the same username", Importance.Low);
            throw new NotFoundException("User not found");
        }

        var oldPasswordHash = PasswordHashProvider.GetHash(oldPassword);
        if (oldPasswordHash != user.PasswordHash)
        {
            await _logService.Create($"Failed to update password for user {username}, because the old password is incorrect", Importance.Medium);
            throw new Exception("Failed to update password");
        }

        user.PasswordHash = PasswordHashProvider.GetHash(newPassword);
        await _context.SaveChangesAsync();
        await _logService.Create($"Password for user {username} successfully updated", Importance.Medium);
        return user;
    }
    
    public async Task<User> UpdateUser(UserDto userDto, string username)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (user == null)
        {
            await _logService.Create($"Failed to update user {username}, because there is none with the same username", Importance.Low);
            throw new NotFoundException("User not found");
        }

        user.FullName = userDto.FullName;
        user.PasswordHash = PasswordHashProvider.GetHash(userDto.Password);

        await _context.SaveChangesAsync();
        await _logService.Create($"User {username} successfully updated", Importance.Medium);
        return user;
    }

    public async Task DeleteUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            await _logService.Create("Failed to delete user with ID {id}, because there is none with the same ID", Importance.Low);
            throw new Exception("User not found");
        }
        await _logService.Create("User with ID {id} successfully deleted", Importance.High);
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }
}