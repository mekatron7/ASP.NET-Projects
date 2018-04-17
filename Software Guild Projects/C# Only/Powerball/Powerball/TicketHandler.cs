using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Powerball
{
    class TicketHandler
    {
        public List<Pick> Tickets { get; set; } = new List<Pick>();
        public int Index { get; set; } = 1;

        public void CreateFromHuman(string filePath)
        {
            string fName = ConsoleInput.GetNameFromHuman("first");
            string lName = ConsoleInput.GetNameFromHuman("last");
            int[] picks = ConsoleInput.GetNumbersFromHuman();

            Pick p = new Pick(fName, lName, picks, Index);
            File.AppendAllText(filePath, $"{Environment.NewLine}{p.FirstName},{p.LastName},{picks[0]}/{picks[1]}/{picks[2]}/{picks[3]}/{picks[4]}/{picks[5]}");
            Tickets.Add(p);
            Index++;
        }

        public void CreateFromFile(string potentialPick)
        {
            Pick p = null;

            string[] pickInfo = potentialPick.Split(',');
            string fName = pickInfo[0];
            string lName = pickInfo[1];
            int[] balls = pickInfo[2].Split('/')
                .Select(i => int.Parse(i))
                .ToArray();
            p = new Pick(fName, lName, balls, Index);
            Tickets.Add(p);
            Index++;
        }

        public IEnumerable<Pick> FindBestMatches(Pick draw)
        {
            List<Pick> winners = new List<Pick>();

            foreach(Pick p in Tickets)
            {
                var matches = p.First5.Intersect(draw.First5);
                
                    switch (matches.Count())
                    {
                        case 1:
                        if (p.PBall == draw.PBall)
                        {
                            p.Winnings = 4;
                        }
                        break;
                        case 2:
                        if (p.PBall == draw.PBall)
                        {
                            p.Winnings = 7;
                        }
                        break;
                        case 3:
                        if (p.PBall == draw.PBall)
                        {
                            p.Winnings = 100;
                        }
                        else
                        {
                            p.Winnings = 7;
                        }
                        break;
                        case 4:
                        if (p.PBall == draw.PBall)
                        {
                            p.Winnings = 50000;
                        }
                        else
                        {
                            p.Winnings = 100;
                        }
                        break;
                        case 5:
                        if (p.PBall == draw.PBall)
                        {
                            p.Winnings = 1996000000;
                        }
                        else
                        {
                            p.Winnings = 1000000;
                        }
                        break;
                        default:
                        if(p.PBall == draw.PBall)
                        {
                            p.Winnings = 4;
                        }
                        break;
                    }
                Console.Write($"Matches for {p.FirstName} {p.LastName}: ");
                foreach (int i in matches)
                {
                    Console.Write($"{i}, ");
                }
                Console.WriteLine();

                if(p.Winnings > 0)
                {
                    winners.Add(p);
                }
            }
            Console.ReadLine();
            Tickets.Clear();
            Index = 1;
            return winners;
        }

        public Pick FindByID(int id)
        {
            return Tickets.FirstOrDefault(i => i.Index == id);
        }

        public void PrintTickets()
        {
            Console.WriteLine();

            foreach(Pick p in Tickets)
            {
                Console.WriteLine(p);
                Console.WriteLine();
            }
            Console.ReadLine();
            Console.Clear();
        }
    }
}
