using RockPaperScissors.BLL;
using RockPaperScissors.BLL.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RockPaperScissors
{
    class Program
    {
        static void Main(string[] args)
        {
            IChoiceGetter p1 = new HumanPlayer();
            IChoiceGetter p2 = new RandomPlayer();

            var engine = new GameEngine(p1, p2);

            int p1Wins = 0;
            int p2Wins = 0;
            int draws = 0;
            for(int i = 0; i < 10; i++)
            {
                RoundResult result = engine.PlayRound();
                Console.WriteLine($"Player 1 played {result.P1Choice}. Player 2 played {result.P2Choice}.");
                if(result.Result < 0)
                {
                    p1Wins++;
                    Console.WriteLine("Player 1 wins!");
                }
                else if(result.Result > 0)
                {
                    p2Wins++;
                    Console.WriteLine("Player 2 wins!");
                }
                else
                {
                    draws++;
                    Console.WriteLine("Both players didn't win shit.");
                }
                Console.WriteLine();
            }
            Console.WriteLine("After 10 games: ");
            Console.WriteLine("Player 1 wins: " + p1Wins);
            Console.WriteLine("Player 2 wins: " + p2Wins);
            Console.WriteLine("Draws: " + draws);
            Console.ReadLine();
        }
    }
}
