using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gematrianator.Models
{
    public class Character
    {
        public char CharacterName { get; set; }
        public Dictionary<string, int> Ciphers { get; set; } = new Dictionary<string, int>();
    }
}