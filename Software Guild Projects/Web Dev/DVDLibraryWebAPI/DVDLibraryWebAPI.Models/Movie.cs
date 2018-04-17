using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVDLibraryWebAPI.Models
{
    public class Movie
    {
        public int MovieId { get; set; }
        public int RatingId { get; set; }
        public int DirectorId { get; set; }
        public string Title { get; set; }
        public int ReleaseYear { get; set; }
    }
}
