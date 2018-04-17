using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Powerball
{
    class QuickPick : IPickRepository
    {
        public List<Pick> Tickets { get; set; } = new List<Pick>();
        public int Index { get; set; } = 1;

        public IEnumerable<Pick> FindBestMatches(Pick draw)
        {
            List<Pick> winners = new List<Pick>();
            int counter = 0;
            int numOfTickets = Tickets.Count();
            foreach(Pick p in Tickets)
            {
                switch (counter)
                {
                    case 0:
                        Console.Clear();
                        Console.WriteLine($"Generating {numOfTickets} tickets.");
                        break;
                    case 500000:
                        Console.Clear();
                        Console.WriteLine($"Generating {numOfTickets} tickets..");
                        break;
                    case 1000000:
                        Console.Clear();
                        Console.WriteLine($"Generating {numOfTickets} tickets...");
                        break;
                    case 1500000:
                        counter = -1;
                        break;
                }

                counter++;

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
                //Console.Write("Matches: ");
                //foreach (int i in matches)
                //{
                //    Console.Write($"{i}, ");
                //}
                //Console.WriteLine();

                if(p.Winnings > 0)
                {
                    winners.Add(p);
                }
            }
            Console.Clear();
            Console.WriteLine($"Finished generating {Tickets.Count()} tickets. Press any key to continue.");
            Console.ReadKey();
            Tickets.Clear();
            Index = 0;
            return winners;
        }

        public Pick FindByID(int id)
        {
            return Tickets.FirstOrDefault(i => i.Index == id);
        }

        public Pick Create(Pick p)
        {
            p.Index = Index;
            p.FirstName = "Player";
            p.LastName = $"#{Index}";
            Index++;
            Tickets.Add(p);
            return p;
        }
    }
}
