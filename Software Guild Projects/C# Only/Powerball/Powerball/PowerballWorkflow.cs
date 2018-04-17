using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Powerball
{
    static class PowerballWorkflow
    {
        public static TicketHandler TH { get; set; } = new TicketHandler();
        public static QuickPick QP { get; set; } = new QuickPick();
        private static string _filePath = @"C:\Data\SystemIO\Powerball Picks.txt";
        public static IEnumerable<Pick> Winners { get; set; }
        public static string Mode { get; set; }

        public static void GetTickets()
        {
            while(true)
            {
                Mode = ConsoleInput.FileOrHuman();
                if (Mode == "F")
                {
                    TicketsFromFile();
                    TH.PrintTickets();
                    LotteryBalls();
                }
                else if (Mode == "H")
                {
                    TicketFromHuman();
                    TH.PrintTickets();
                }
                else if(Mode == "M")
                {
                    MassTicketGenerator(ConsoleInput.GetAmountOfTickets());
                    LotteryBalls();
                }
                else
                {
                    break;
                }
            }          
        }

        public static void TicketsFromFile()
        {
            if (!File.Exists(_filePath))
            {
                Console.WriteLine("That file doesn't exist.");
            }
            else
            {
                string[] potentialPicks = File.ReadAllLines(_filePath);

                foreach (string pick in potentialPicks)
                {
                    TH.CreateFromFile(pick);
                }
            }
        }

        public static void TicketFromHuman()
        {
            TH.CreateFromHuman(_filePath);
        }

        public static void MassTicketGenerator(int numberOfTickets)
        {
            Console.WriteLine();
            Console.WriteLine("Loading...");
            int[] randomChoices = new int[6];
            int ball = 0;

            for(int i = 0; i < numberOfTickets; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    if (j < 5)
                    {
                        ball = RNG.NextInt(1, 70);
                        if (randomChoices.Contains(ball))
                        {
                            j--;
                            continue;
                        }
                        randomChoices[j] = ball;
                    }
                    else
                    {
                        randomChoices[j] = RNG.NextInt(1, 27);
                    }
                }
                Pick randomPick = new Pick(randomChoices);
                QP.Create(randomPick);
            }
        }

        public static void LotteryBalls()
        {
            int[] winningBalls = new int[6];
            int ball = 0;
            for(int i = 0; i < 6; i++)
            {
                if (i < 5)
                {
                    ball = RNG.NextInt(1, 70);
                    if (winningBalls.Contains(ball))
                    {
                        i--;
                        continue;
                    }
                    winningBalls[i] = ball;
                }
                else
                {
                    winningBalls[i] = RNG.NextInt(1, 27);
                }
            }
            Pick winningPick = new Pick(winningBalls);
            IEnumerable<Pick> bestMatches = null;
            if(Mode == "M")
            {
                bestMatches = QP.FindBestMatches(winningPick).OrderBy(i => i.Winnings);
            }
            else if(Mode == "F" || Mode == "H")
            {
                bestMatches = TH.FindBestMatches(winningPick).OrderBy(i => i.Winnings);
            }
            Winners = bestMatches;
            DisplayWinners(winningPick);
        }

        public static void DisplayWinners(Pick winningPick)
        {
            int winnersCount = Winners.Count();
            if(winnersCount > 50000)
            {
                Winners = Winners.Where(i => i.Winnings > 7);
            }

            Console.Clear();
            Console.WriteLine("Lottery winners:");
            if(winnersCount == 0)
            {
                Console.WriteLine("Nobody won anything. Everyone stays poor this round.");
            }
            else
            {
                foreach (Pick p in Winners)
                {
                    Console.WriteLine($"Winning Pick: {winningPick.BallsArrayToString()} : Powerball: {winningPick.PBall}");
                    Console.WriteLine(p);
                    Console.WriteLine();
                }
            }
            
            Console.ReadKey();
            Console.Clear();
        }
    }
}
