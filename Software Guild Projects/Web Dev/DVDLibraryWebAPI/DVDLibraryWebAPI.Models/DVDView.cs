using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DVDLibraryWebAPI.Models
{
    public class DVDView
    {
        [Key]
        public int DVDId { get; set; }
        public string Title { get; set; }
        public string Rating { get; set; }
        public string Genre { get; set; }
        public string Director { get; set; }
        public int ReleaseYear { get; set; }
        public string Notes { get; set; }
    }
}