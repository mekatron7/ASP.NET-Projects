using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LinkFinderApp.Models
{
    public class LinkFinderVM
    {
        public string URL { get; set; }
        public string Content { get; set; }
        public List<string> Links { get; set; } = new List<string>();
        public int LinksTotal { get; set; }
        public List<string> ErrorMessages { get; set; } = new List<string>();
        public int NumOfLevels { get; set; }
        public int NumOfLinks { get; set; }
        public bool AllLinksFromFirstPage { get; set; }
        public bool KeepLinkStyles { get; set; }
    }
}