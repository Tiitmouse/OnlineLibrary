using Data.Dto;
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

    public UserServices(IConfiguration configuration, RwaContext context)
    {
        _configuration = configuration;
        _context = context;
    }

    public async Task<User> Register(User user, string password)
    {
        var trimmedUsername = user.Username.Trim();
        if (await _context.Users.AnyAsync(x => x.Username.Equals(trimmedUsername)))
            throw new Exception($"Username {trimmedUsername} already exists");

        var b64hash = PasswordHashProvider.GetHash(password);
        user.PasswordHash = b64hash;

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return user;
    }

    public async Task<string> Login(string username, string password)
    {
        var genericLoginFail = "Incorrect username or password";

        var existingUser = await _context.Users.FirstOrDefaultAsync(x => x.Username == username);
        if (existingUser == null)
            throw new Exception(genericLoginFail);

        var b64hash = PasswordHashProvider.GetHash(password);
        if (b64hash != existingUser.PasswordHash)
            throw new Exception(genericLoginFail);

        var secureKey = _configuration["JWT:SecureKey"];
        return JwtTokenProvider.CreateToken(secureKey, 120, username);
    }

    public async Task<User> GetUser(string username)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == username);
        if (user == null)
            throw new NotFoundException("User not found");

        return user;
    }

    public async Task<User> UpdateUser(UserDto userDto, string username)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (user == null)
            throw new NotFoundException("User not found");

        user.FullName = userDto.FullName;
        user.PasswordHash = PasswordHashProvider.GetHash(userDto.Password);

        await _context.SaveChangesAsync();

        return user;
    }

    public async Task DeleteUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
            throw new Exception("User not found");

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }
}