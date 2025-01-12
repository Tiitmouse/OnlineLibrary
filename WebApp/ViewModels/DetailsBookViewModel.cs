using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace WebApp.Models
{
    public class DetailsBookViewModel
    {
        public int IdBook { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public int? PublicationYear { get; set; }
        public string Isbn { get; set; }
        public string AuthorName { get; set; }
        public string GenreName { get; set; }
        public List<LibraryAvailabilityViewModel> Libraries { get; set; }

        public IEnumerable<SelectListItem> Genres { get; set; }
        public IEnumerable<SelectListItem> Authors { get; set; }
    }
}