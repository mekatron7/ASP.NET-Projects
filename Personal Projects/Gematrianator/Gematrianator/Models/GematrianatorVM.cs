using Gematrianator.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gematrianator.Models
{
    public class GematrianatorVM
    {
        public string UserInput { get; set; }
        public bool EnglishOrdinal { get; set; }
        public bool ReverseEnglishOrdinal { get; set; }
        public bool EnglishFullReduction { get; set; }
        public bool ReverseEnglishFullReduction { get; set; }
        public int EnglishOrdinalNum { get; set; }
        public int ReverseEnglishOrdinalNum { get; set; }
        public int EnglishFullReductionNum { get; set; }
        public int ReverseEnglishFullReductionNum { get; set; }
        public List<WordCipher> DecodedWords { get; set; }
    }
}