namespace WebApp.Models;

public class UserDetailsViewModel
{
    public int IdUser { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } 
    public string? FullName { get; set; }
}