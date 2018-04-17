using BattleShip.BLL.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BattleShip.UI
{
    static class ConsoleInput
    {
        internal static string GetPlayerSide()
        {
            bool sidesChosen = false;
            string side = "";
            while (!sidesChosen)
            {
                Console.WriteLine($"Choose your destiny. \nType 'J' for Jedi or 'S' for Sith., then hit Enter.");
                string input = Console.ReadLine().ToUpper();
                Console.WriteLine();

                if (input == "J" || input == "S")
                {
                    if (input == "J")
                    {
                        side = "Jedi";
                    }
                    else
                    {
                        side = "Sith";

                    }
                    sidesChosen = true;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("You lack discipline, young padawan. Please enter a valid choice.");
                }
            }

            return side;
        }

        internal static string GetPlayerName(string side)
        {
            string name = "";
            
            while(name == "")
            {
                Console.Clear();
                if (side == "Jedi")
                {
                    Console.WriteLine($"Young padawan, type in your name and hit Enter.");
                }
                else
                {
                    Console.WriteLine($"Sith apprentice, type in your name and hit Enter.");
                }
                Console.Write("Name: ");
                name = Console.ReadLine();
                Console.Clear();
            }

            return name;
        }

        public static Coordinate GetCoordinates()
        {
            bool isValid = false;
            Coordinate c = null;
            string input = "";

            while (!isValid)
            {
                Console.Write("Coordinates: ");
                input = Console.ReadLine().ToUpper();

                if(input == "I")
                {
                    ConsoleOutput.SetupInstructions();
                }
                else if(input != "")
                {
                    string first = input.Substring(0, 1);
                    string second = input.Substring(1);

                    int firstCoord = first[0] - 'A' + 1;
                    int secondCoord;

                    if (firstCoord <= 0 || firstCoord > 10)
                    {
                        Console.WriteLine("Please enter a letter ranging from A-J followed by a number ranging from 1-10. (A7)");
                    }
                    else
                    {
                        if (!int.TryParse(second, out secondCoord))
                        {
                            Console.WriteLine("That ain't even a coordinate tho. Please enter in a real coordinate.");
                        }
                        else if (secondCoord <= 0 || secondCoord > 10)
                        {
                            Console.WriteLine("Please enter a coordinate number ranging from 1-10");
                        }
                        else
                        {
                            isValid = true;
                            c = new Coordinate(firstCoord, secondCoord);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("You gotta gimme something bro.");
                }
                
            }

            return c;

            //for(char c = 'A'; c <= 'Z'; c++)
            //{
            //    int numberEquivalent = c - 'A' + 1;
            //    Console.WriteLine("First coorinate:" + first + " which translates to: " + numberEquivalent);
            //}
        }

        internal static ShipDirection GetDirection()
        {
            bool isValid = false;
            ShipDirection sd = ShipDirection.Right;
            string direction = "";

            Console.WriteLine("Now choose what direction your ship is facing.");
            Console.WriteLine("U = Up\nD = Down\nL = Left\nR = Right");
            Console.WriteLine("You can also just type in the literal direction (ie. \"Up\" \"Left\")");
            while (!isValid)
            {
                Console.Write("Direction choice: ");
                direction = Console.ReadLine().ToUpper();

                switch (direction)
                {
                    case "U":
                    case "UP":
                        sd = ShipDirection.Up;
                        isValid = true;
                        break;
                    case "D":
                    case "DOWN":
                        sd = ShipDirection.Down;
                        isValid = true;
                        break;
                    case "L":
                    case "LEFT":
                        sd = ShipDirection.Left;
                        isValid = true;
                        break;
                    case "R":
                    case "RIGHT":
                        sd = ShipDirection.Right;
                        isValid = true;
                        break;
                    default:
                        Console.WriteLine();
                        Console.WriteLine("Young padawan quit wasting time and select a valid direction.");
                        break;
                }
            }
            return sd;
        }
    }
}
