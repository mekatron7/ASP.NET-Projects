using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RockPaperScissors.BLL
{
    public class RoundResult
    {
        public RPSChoice P1Choice { get; }
        public RPSChoice P2Choice { get; }
        public int Result { get; }

        public RoundResult(RPSChoice p1Choice, RPSChoice p2Choice, int result)
        {
            P1Choice = p1Choice;
            P2Choice = p2Choice;
            Result = result;
        }
    }
}
