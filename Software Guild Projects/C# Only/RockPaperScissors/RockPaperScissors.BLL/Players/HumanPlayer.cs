using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RockPaperScissors.BLL.Players
{
    public class HumanPlayer : IChoiceGetter
    {
        public RPSChoice GetChoice()
        {
            RPSChoice playerChoice = RPSChoice.Paper;

            bool validInput = false;

            while (!validInput)
            {
                Console.WriteLine("Please enter choice (R/P/S): ");
                string playerEntry = Console.ReadLine().ToUpper();

                switch (playerEntry)
                {
                    case "R":
                        playerChoice = RPSChoice.Rock;
                        validInput = true;
                        break;
                    case "P":
                        playerChoice = RPSChoice.Paper;
                        validInput = true;
                        break;
                    case "S":
                        playerChoice = RPSChoice.Scissors;
                        validInput = true;
                        break;
                    default:
                        Console.WriteLine("That shit's invalid af.");
                        break;
                }
            }

            return playerChoice;
        }
    }
}
