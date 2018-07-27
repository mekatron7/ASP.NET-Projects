using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PriceConfigApp.Models
{
    public class PriceConfigVM
    {
        public HttpPostedFileBase CSVFile { get; set; }

        public decimal NewPrice { get; set; }

        public int PriceId { get; set; }

        public bool Uploaded { get; set; }

        public List<PriceItem> Prices { get; set; }

        public string Mode { get; set; }

        public string ErrorMessage { get; set; }
    }
}