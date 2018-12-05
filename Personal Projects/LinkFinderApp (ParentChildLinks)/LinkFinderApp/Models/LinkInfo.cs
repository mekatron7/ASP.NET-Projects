using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LinkFinderApp.Models
{
    public class LinkInfo
    {
        public string LinkContent { get; set; }
        public string LinkAddress { get; set; }
        public LinkInfo ParentLink { get; set; }
        public List<LinkInfo> ChildLinks { get; set; }
    }
}