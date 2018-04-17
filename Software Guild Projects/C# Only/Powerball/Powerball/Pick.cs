using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Powerball
{
    class Pick
    {
        public int[] First5 { get; set; }
        public int PBall { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Index { get; set; }
        public decimal Winnings { get; set; } = 0;

        public Pick(string fName, string lName, int[] picks, int index)
        {
            First5 = picks.Take(5).ToArray();
            PBall = picks[5];
            FirstName = fName;
            LastName = lName;
            Index = index;
        }

        public Pick(int[] picks)
        {
            First5 = picks.Take(5).ToArray();
            PBall = picks[5];
        }

        public override string ToString()
        {
            return $"Ticket Number: {Index} // Name: {FirstName} {LastName}\nFirst 5 balls: {BallsArrayToString()} // Powerball: {PBall}\nWinnings: {Winnings:c}";
        }

        public string BallsArrayToString()
        {
            return $"[{First5[0]}, {First5[1]}, {First5[2]}, {First5[3]}, {First5[4]}]";
        }
    }
}
