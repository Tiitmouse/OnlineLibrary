using Data.Dto;
using Data.Eunumerators;
using Data.Exceptions;
using Data.Models;
using Data.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Data.Services;

public interface IUserServices
{
    Task<User> Register(User user, string password);
    Task<string> Login(string username, string password);
    Task<User> GetUser(string username);
    Task<User> UpdateUser(UserDto userDto, string username);
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
            _logService.Create("User failed to register", Importance.Low);
            throw new Exception($"Username {trimmedUsername} already exists");
        }
        
        var b64hash = PasswordHashProvider.GetHash(password);
        user.PasswordHash = b64hash;

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        _logService.Create($"User {user.Username} successfully registered", Importance.Low);
        return user;
    }

    public async Task<string> Login(string username, string password)
    {
        var genericLoginFail = "Incorrect username or password";

        var existingUser = await _context.Users.FirstOrDefaultAsync(x => x.Username == username);
        if (existingUser == null)
        { 
            _logService.Create("User failed to log in", Importance.Medium);
            throw new Exception(genericLoginFail);
        }

        var b64hash = PasswordHashProvider.GetHash(password);
        if (b64hash != existingUser.PasswordHash)
        { 
            _logService.Create("User failed to log in", Importance.Medium);
            throw new Exception(genericLoginFail);
        }


        var secureKey = _configuration["JWT:SecureKey"];
        _logService.Create($"User {username} successfully logged in", Importance.Low);
        return JwtTokenProvider.CreateToken(secureKey, 120, username);
    }

    public async Task<User> GetUser(string username)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == username);
        if (user == null)
        {
            _logService.Create("Failed to fetch user", Importance.Low);
            throw new NotFoundException("User not found");
        }

        _logService.Create("User fetched successfully", Importance.Low);
        return user;
    }

    public async Task<User> UpdateUser(UserDto userDto, string username)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (user == null)
        {
            _logService.Create($"Failed to update user {username}, because there is none with the same username", Importance.Low);
            throw new NotFoundException("User not found");
        }

        user.FullName = userDto.FullName;
        user.PasswordHash = PasswordHashProvider.GetHash(userDto.Password);

        await _context.SaveChangesAsync();
        _logService.Create($"User {username} successfully updated", Importance.Medium);
        return user;
    }

    public async Task DeleteUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            _logService.Create("Failed to delete user with ID {id}, because there is none with the same ID", Importance.Low);
            throw new Exception("User not found");
        }
        _logService.Create("User with ID {id} successfully deleted", Importance.High);
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }
}