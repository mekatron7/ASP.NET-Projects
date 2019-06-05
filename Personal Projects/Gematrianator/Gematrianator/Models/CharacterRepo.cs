using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gematrianator.Models
{
    public class CharacterRepo
    {
        public bool Set { get; set; }
        public List<Character> CharList { get; set; } = new List<Character>()
        {
            new Character{CharacterName = 'A'},
            new Character{CharacterName = 'B'},
            new Character{CharacterName = 'C'},
            new Character{CharacterName = 'D'},
            new Character{CharacterName = 'E'},
            new Character{CharacterName = 'F'},
            new Character{CharacterName = 'G'},
            new Character{CharacterName = 'H'},
            new Character{CharacterName = 'I'},
            new Character{CharacterName = 'J'},
            new Character{CharacterName = 'K'},
            new Character{CharacterName = 'L'},
            new Character{CharacterName = 'M'},
            new Character{CharacterName = 'N'},
            new Character{CharacterName = 'O'},
            new Character{CharacterName = 'P'},
            new Character{CharacterName = 'Q'},
            new Character{CharacterName = 'R'},
            new Character{CharacterName = 'S'},
            new Character{CharacterName = 'T'},
            new Character{CharacterName = 'U'},
            new Character{CharacterName = 'V'},
            new Character{CharacterName = 'W'},
            new Character{CharacterName = 'X'},
            new Character{CharacterName = 'Y'},
            new Character{CharacterName = 'Z'}
        };

        public void SetAll()
        {
            SetEnglishOrdinal();
            SetReverseEnglishOrdinal();
            SetEnglishFullReduction();
            SetReverseEnglishFullReduction();
            Set = true;
        }

        private void SetEnglishOrdinal()
        {
            for(int i = 0; i < 26; i++)
            {
                CharList[i].Ciphers.Add("EO", i + 1);
            }
        }

        private void SetReverseEnglishOrdinal()
        {
            int j = 1;
            for (int i = 25; i >= 0; i--)
            {
                CharList[i].Ciphers.Add("REO", j);
                j++;
            }
        }

        private void SetEnglishFullReduction()
        {
            for (int i = 0; i < 26; i++)
            {
                if (i >= 18 && i < 26)
                    CharList[i].Ciphers.Add("EFR", i - 17);
                else if (i >= 9 && i < 18)
                    CharList[i].Ciphers.Add("EFR", i - 8);
                else
                    CharList[i].Ciphers.Add("EFR", i + 1);
            }
        }

        private void SetReverseEnglishFullReduction()
        {
            int j = 1;
            for (int i = 25; i >= 0; i--)
            {
                if(i >= 0 && i < 8)
                CharList[i].Ciphers.Add("REFR", j - 18);
                else if(i >= 8 && i < 17)
                    CharList[i].Ciphers.Add("REFR", j - 9);
                else
                    CharList[i].Ciphers.Add("REFR", j);
                j++;
            }
        }
    }
}