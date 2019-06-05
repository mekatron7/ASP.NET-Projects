using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gematrianator.Models
{
    public class WordCipherInfo
    {
        public string CipherID { get; set; }
        public string CipherName { get; set; }
        public int CipherValue { get; set; }
        public string WordText { get; set; }
    }
}