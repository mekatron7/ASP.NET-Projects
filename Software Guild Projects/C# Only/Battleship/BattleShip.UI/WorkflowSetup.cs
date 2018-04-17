using BattleShip.BLL.GameLogic;
using BattleShip.BLL.Requests;
using BattleShip.BLL.Responses;
using BattleShip.BLL.Ships;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShip.UI
{
    static class WorkflowSetup
    {
        private static Random _rng = new Random();
        public static GameState GameData { get; set; }

        public static void SetupPlayer()
        {
            string side = ConsoleInput.GetPlayerSide();
            Player p1 = BuildPlayer(side, null);
            Player p2 = BuildPlayer(side == "Jedi" ? "Sith":"Jedi", p1); //Terinary operator: Gives p2 whatever p1 side isn't. Does side = "Jedi"? If yes, side = "Sith". If not, side = "Jedi".
            GameData = new GameState(p1, p2);
        }

        private static Player BuildPlayer(string side, Player player)
        {
            //Displays message after picking a side
            if(side == "Jedi")
            {
                if (player == null)
                {
                    Console.WriteLine($"You made a wise choice. Your opponent is weak-minded and therefore has succumbed to the dark side.");
                    Console.WriteLine();
                    Console.WriteLine("Press Enter to continue.");
                }
                else
                {
                    Console.WriteLine($"Player 2, the force has chosen you to save the galaxy from the evil clutches of the Sith Order.\nIt's your destiny to destroy {player.Name}!");
                    Console.WriteLine();
                    Console.WriteLine("Press Enter to continue.");
                }
            }
            else
            {
                if (player == null)
                {
                    Console.WriteLine($"Giving in to the dark side will make you realize your true power!");
                    Console.WriteLine();
                    Console.WriteLine("Press Enter to continue.");
                }
                else
                {
                    Console.WriteLine($"Player 2, you have given in to the sinister ways of the dark side.\n{player.Name} is on a mission to destroy you and your fleet.\nKill them before they kill you!");
                    Console.WriteLine();
                    Console.WriteLine("Press Enter to continue.");
                }
            }
            Console.ReadLine();

            //Gets the players names
            string name = ConsoleInput.GetPlayerName(side);
            Board board = SetupBoard(name, side);
            Player p = new Player(name, side, board);

            return p;
        }

        private static Board SetupBoard(string name, string side)
        {
            Console.WriteLine($"{name}, place your five ships on your board.");
            ConsoleOutput.SetupInstructions();
            Console.WriteLine();

            Board b = new Board();
            string[] placedShips = new string[5];
            string placedLocation = "";
            string placedLocationShip = "";
            ShipType firstShip = ShipType.XWing;
            ShipType lastship = ShipType.JediStarDestroyer;
            if(side == "Sith")
            {
                firstShip = ShipType.TieFighter;
                lastship = ShipType.SithStarDestroyer;
            }

            //This places the ships on the board
            for (ShipType s = firstShip; s <= lastship; s++)
                {
                Console.WriteLine($"{name}, select a deployment location for your {s.ToString()}.");
                PlaceShipRequest request = new PlaceShipRequest(); //makes new ship placement request

                //Get user input for coordinates and ship position
                request.Coordinate = ConsoleInput.GetCoordinates();
                Console.WriteLine();
                request.Direction = ConsoleInput.GetDirection();
                request.ShipType = s;

                    ShipPlacement success = b.PlaceShip(request);
                    if (success != ShipPlacement.Ok)
                    {
                    if(success == ShipPlacement.Overlap)
                    {
                        Console.WriteLine($"Please place your {s.ToString()} in a location that doesn't overlap another ship.");
                        Console.WriteLine();
                    }
                    else //Not Enough Space
                    {
                        Console.WriteLine($"Please place your {s.ToString()} within the bounds of the board.");
                        Console.WriteLine();
                    }

                        s--;
                    }
                else
                {
                    //Shows the player the details of their ship placement
                    placedLocation = $"({(char)(request.Coordinate.XCoordinate + 'A' - 1)}, {request.Coordinate.YCoordinate}) : {request.Direction.ToString()}";
                    placedLocationShip = $"({(char)(request.Coordinate.XCoordinate + 'A' - 1)}, {request.Coordinate.YCoordinate}) : {request.Direction.ToString()} : {s.ToString()}";
                    int placedShipsIndex = 0;
                    if(side == "Jedi")
                    {
                        placedShipsIndex = (int)s;
                    }
                    else
                    {
                        placedShipsIndex = (int)s - 5;
                    }
                    placedShips[placedShipsIndex] = placedLocationShip;
                    Console.WriteLine();
                    Console.WriteLine($"****{s.ToString()} was placed at {placedLocation}****");
                    Console.WriteLine();
                    Console.WriteLine($"{name}'s fleet:");

                    for(int i = 0; i < placedShipsIndex + 1; i++)
                    {
                        Console.WriteLine(placedShips[i]);
                    }
                    
                    Console.WriteLine();
                    Console.WriteLine("Press Enter to continue.");
                    Console.ReadLine();
                    Console.Clear();
                }
                }
            Console.WriteLine($"{name}'s fleet is all set and ready for deployment!\n\nPress Enter to continue.");
            Console.ReadLine();
            Console.Clear();
            return b;
        }

        public static void StartMatch()
        {
            Console.WriteLine("Galactic warfare is at hand. Use the force to vanquish your opponent!");
            Console.WriteLine();
            Console.WriteLine("Press Enter to continue.");
            Console.ReadLine();
            int whoFirst = _rng.Next(2);
            bool gameOver = false;
            Player currentPlayer = null;
            Player target = null;
            FireShotResponse fsr = null;

            if(whoFirst == 0)
            {
                currentPlayer = GameData.P1;
                target = GameData.P2;
            }
            else
            {
                currentPlayer = GameData.P2;
                target = GameData.P1;
            }

            while(!gameOver)
            {
                Console.Clear();
                ConsoleOutput.DisplayBoard(currentPlayer, target);

                fsr = Fire(currentPlayer, target);

                switch (fsr.ShotStatus)
                {
                    case ShotStatus.Hit:
                        Console.WriteLine($"You hit one of {target.Name}'s ships! {target.Name} is pissed!");
                        break;
                    case ShotStatus.Miss:
                        Console.WriteLine("You missed!! The force is not strong with you.");
                        break;
                    case ShotStatus.Invalid:
                        Console.WriteLine("That's not a valid hit coordinate. Try again bud.");
                        Console.WriteLine();
                        continue;
                    case ShotStatus.Duplicate:
                        Console.WriteLine($"Way to go {currentPlayer.Name}, you hit a location you already hit.\nThat's like, $2000 down the drain in wasted ammo.");
                        Console.WriteLine();
                        Console.WriteLine("Press Enter to try again.");
                        Console.ReadLine();
                        continue;
                    case ShotStatus.HitAndSunk:
                        if(_rng.Next(4) >= 2 && currentPlayer.Side == "Jedi")
                        {
                            Console.WriteLine($"Mace Windu: \"I have had it with these muthaf****n Sith on this muthaf****n ship!\"");
                            Console.WriteLine($"Mace Windu did a drive by shooting on {target.Name}'s {fsr.ShipImpacted} in a stolen A-Wing and blew that bitch up!");
                        }
                        else
                        {
                            Console.WriteLine($"You blew up {target.Name}'s {fsr.ShipImpacted} and killed everyone in there!");
                        }
                        break;
                    case ShotStatus.Victory:
                        Console.WriteLine($"Kudos {currentPlayer.Name}, you mercilessly murdered all of {target.Name}'s crew in cold blood!!");
                        gameOver = true;
                        Console.WriteLine();
                        Console.WriteLine("Press Enter to continue.");
                        Console.ReadLine();
                        break;
                }

                if (!gameOver)
                {
                    Console.WriteLine();
                    Console.WriteLine($"{target.Name}, you're up. Press Enter to continue.");
                    Console.ReadLine();

                    //Switches to the other player
                    if (currentPlayer == GameData.P1)
                    {
                        currentPlayer = GameData.P2;
                        target = GameData.P1;
                    }
                    else
                    {
                        currentPlayer = GameData.P1;
                        target = GameData.P2;
                    }
                }
            }
            if (ConsoleOutput.VictoryMessage(currentPlayer))
            {
                SetupPlayer();
                StartMatch();
            }        
        }

        public static FireShotResponse Fire(Player attacker, Player target)
        {
            Coordinate c = null;
            Console.WriteLine($"{attacker.Name}, select a valid coordinate to fire your shot!");
            Console.Write("Attack ");
            c = ConsoleInput.GetCoordinates();
            string attackCoords = $"({(char)(c.XCoordinate + 'A' - 1)}, {c.YCoordinate})";
            FireShotResponse fsr = target.PlayerBoard.FireShot(c);
            Console.WriteLine();
            Console.WriteLine($"{attacker.Name} chose to attack {target.Name}'s battlezone at {attackCoords}!");
            Console.WriteLine();

            return fsr;
        }
    }
}
