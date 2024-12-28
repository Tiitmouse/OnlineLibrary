using System.ComponentModel.DataAnnotations;
namespace Data.Dto;

public class UserDto
{
    public int IdUser { get; set; }
    
    [Required(ErrorMessage = "User name is required")]
    public string Username { get; set; } = null!;
    
    [Required(ErrorMessage = "Password is required")]
    [StringLength(256, MinimumLength = 8, ErrorMessage = "Password should be at least 8 characters long")]
    public string Password { get; set; } = null!;

    [Required(ErrorMessage = "Full name is required")]
    public string FullName { get; set; }
}