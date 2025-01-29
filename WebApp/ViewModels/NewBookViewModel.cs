using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class NewBookViewModel
    {
        public int IdBook { get; set; }
        [Required(ErrorMessage = "it would be nice to have a title, dontcha think?")]
        public string Title { get; set; }
        [Required(ErrorMessage = "how would people know what is this about?")]
        public string? Description { get; set; }
        [Required(ErrorMessage = "when was this published?")]
        [Range(1455, 2025, ErrorMessage = "Publication year must be between 1455 and 2025")]
        public int? PublicationYear { get; set; }
        [Required(ErrorMessage = "ISBN is a must have for a book to exist in this world of ours (or any other)")]
        [RegularExpression(@"^\d{13}$", ErrorMessage = "ISBN is 13 digits long")]
        public string Isbn { get; set; }
        [Required(ErrorMessage = "who wrote this? a ghost?")]
        public string Author{ get; set; }
        [Required(ErrorMessage = "what genre is this? a mystery?")]
        public string Genre { get; set; }
        
        public List<LibraryAvailabilityViewModel> Libraries { get; set; }
        public IEnumerable<SelectListItem> Genres { get; set; }
        public IEnumerable<SelectListItem> Locations { get; set; }
        public IEnumerable<SelectListItem> Authors { get; set; }
        public List<int> LocationIds { get; set; } 

    }
}