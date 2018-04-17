using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Powerball
{
    static class ConsoleInput
    {
        public static string FileOrHuman()
        {
            bool isValid = false;
            string input = "";

            while (!isValid)
            {
                Console.WriteLine("\"If you want to win the lottery, you gotta make the money\nto buy the ticket.\"\n       - That guy from Nightcrawler");
                Console.WriteLine();
                Console.WriteLine("Type 'F' to add tickets from a file.\nType 'H' to manually enter tickets you human.\nType 'M' to mass produce a bunch of random picks.\nType 'Q' to quit.");
                Console.WriteLine();
                Console.Write("Choice: ");
                input = Console.ReadLine().ToUpper();

                if (input == "F" || input == "H" || input == "M" || input == "Q")
                {
                    isValid = true;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("You wanna win hella money right? Then type 'F', 'H', or 'M' goddammit.");
                    Console.WriteLine("Press Enter to continue.");
                    Console.ReadLine();
                    Console.Clear();
                }
            }
            return input;
        }

        public static int GetAmountOfTickets()
        {
            int amount = 0;
            bool isVhaled = false;
            while (!isVhaled)
            {
                Console.Clear();
                Console.WriteLine("Enter the amount of tickets you want to generate");
                Console.Write("Amount: ");
                string input = Console.ReadLine();
                if (int.TryParse(input, out amount))
                {
                    isVhaled = true;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Yo, enter in an actual number, fuckin stupid idiot!");
                    Console.ReadLine();
                }
            }
            return amount;
        }

        public static string GetNameFromHuman(string ForL)
        {
            string name = "";

            while (name == "")
            {
                Console.Clear();
                Console.WriteLine($"Please enter your {ForL} name: ");
                name = Console.ReadLine();
            }

            return name;
        }

        public static int[] GetNumbersFromHuman()
        {
            int[] choices = new int[6];
            string input;

            for(int i = 0; i < 6; i++)
            {
                Console.Clear();
                if(i < 5)
                {
                    Console.WriteLine("Pick a number between 1 and 69.");
                    Console.Write($"Ball {i + 1}: ");
                }
                else
                {
                    Console.WriteLine("Pick a number between 1 and 26.");
                    Console.Write($"Ball {i + 1} (Powerball): ");
                }

                input = Console.ReadLine();
                int ball = 0;
                if(int.TryParse(input, out ball))
                {
                    if(i < 5)
                    {
                        if(ball >= 1 && ball <= 69)
                        {
                            if (choices.Contains(ball))
                            {
                                i--;
                                Console.WriteLine();
                                Console.WriteLine("You can't have duplicate numbers in the first five.");
                                Console.WriteLine("Press Enter to continue.");
                                Console.ReadLine();
                                continue;
                            }
                            else
                            {
                                choices[i] = ball;
                            }        
                        }
                        else
                        {
                            i--;
                            Console.WriteLine();
                            Console.WriteLine("You gotta pick a number between 1 and 69");
                            Console.WriteLine("Press Enter to continue.");
                            Console.ReadLine();
                            continue;
                        }
                    }
                    else
                    {
                        if(ball >= 1 && ball <= 26)
                        {
                            choices[i] = ball;
                        }
                        else
                        {
                            i--;
                            Console.WriteLine();
                            Console.WriteLine("You gotta pick a number between 1 and 26");
                            Console.WriteLine("Press Enter to continue.");
                            Console.ReadLine();
                            continue;
                        }
                    }
                }
                else
                {
                    i--;
                    Console.WriteLine();
                    Console.WriteLine("You gotta pick a number between 1 and 69 for the first 5 balls." +
                        "\nFor the Powerball, pick a number between 1 and 26");
                    Console.WriteLine("Press Enter to continue.");
                    Console.ReadLine();
                    continue;
                }
            }

            return choices;
        }
    }
}
