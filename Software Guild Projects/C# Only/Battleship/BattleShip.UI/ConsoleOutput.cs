using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleShip.BLL.Responses;
using BattleShip.BLL.Requests;

namespace BattleShip.UI
{
    static class ConsoleOutput
    {
        public static void DisplayBoard(Player current, Player target)
        {
            Console.WriteLine($"{current.Name}'s shot history board:");
            Console.WriteLine();
            string hit = " H ";
            string miss = " M ";
            string unknown = " + ";
            char rowLetter = (char)('A' - 1);
            Coordinate c = null;
            ShotHistory sh = ShotHistory.Unknown;
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(" | 1  2  3  4  5  6  7  8  9  10");
            Console.ResetColor();
            for(int row = 1; row <= 10; row++)
            {
                rowLetter++;
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write($"{rowLetter}|");
                Console.ResetColor();
                for(int col = 1; col <= 10; col++)
                {
                    c = new Coordinate(row, col);
                    sh = target.PlayerBoard.CheckCoordinate(c);

                    if (sh == ShotHistory.Hit)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(hit);
                        Console.ResetColor();
                    }
                    else if(sh == ShotHistory.Miss)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write(miss);
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.Write(unknown);
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public static void SetupInstructions()
        {
            Console.WriteLine();
            Console.WriteLine("Enter in a coordinate in the form of A10 (letter first, then number after).");
            Console.WriteLine("Letters (rows) go from A-J going from top to bottom.");
            Console.WriteLine("Numbers (columns) go from 1-10 going from left to right.");
            Console.WriteLine("Type \"I\" and press Enter to see these instructions again.");
        }

        public static bool VictoryMessage(Player winner)
        {
            bool playAgain = false;

            Console.Clear();
            if(winner.Side == "Jedi")
            {
                Console.WriteLine($"Good news {winner.Name}, the Jedi Council just hit you up and is very pleased with your use of the force.\nThey're gonna save a seat for you on the council!");
            }
            else
            {
                Console.WriteLine($"{winner.Name}, your harnessing of the dark side of the force hasn't gone unnoticed.\nPope Benedict XVI...err...I mean Darth Sidious will greatly reward your\ncontributions to the Sith Order with $1,000,000 personally wired to\nyour Venmo account.");
            }
            Console.WriteLine();
            Console.WriteLine("Press Enter to continue.");
            Console.ReadLine();
            Console.Clear();
            Console.WriteLine($"Don't get too comfortable {winner.Name}, more galactic conquests await!\n\nWould you like to take part in another battle?\nType 'Y' for Yes or 'N' for No and press Enter.");

            bool isValid = false;

            while (!isValid)
            {
                Console.WriteLine();
                Console.WriteLine("Battle again?: ");
                string input = Console.ReadLine().ToUpper();

                if (input == "Y")
                {
                    playAgain = true;
                    isValid = true;
                }
                else if (input == "N")
                {
                    isValid = true;
                }
                else
                {
                    Console.WriteLine("If you're smart enough to kill an entire fleet, you're smart enough to type 'Y' or 'N'.");
                }
            }
            return playAgain;
        }
    }
}
