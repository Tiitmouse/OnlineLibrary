using System.ComponentModel.DataAnnotations;

namespace WebApp.Models;

public class RatingViewModel
{
    public int IdRating { get; set; }
    public int UserId { get; set; }

    public int BookId { get; set; }

    [RegularExpression(@"^[1-5]$", ErrorMessage = "Rating must be between 1 and 5")]
    public int Rating { get; set; }
    public string? Comment { get; set; }
    
    public string? Username { get; set; }

}