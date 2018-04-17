using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RockPaperScissors.BLL.Players
{
    public class RandomPlayer : IChoiceGetter
    {
        public RPSChoice GetChoice()
        {
            int choice = RNG.NextInt(0, 3);

            RPSChoice toReturn = RPSChoice.Paper;

            switch (choice)
            {
                case 0:
                    toReturn = RPSChoice.Paper;
                    break;
                case 1:
                    toReturn = RPSChoice.Rock;
                    break;
                case 2:
                    toReturn = RPSChoice.Scissors;
                    break;
            }

            return toReturn;
        }
    }
}
