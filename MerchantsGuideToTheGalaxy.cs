using System;
using System.Collections.Generic;
using System.IO;

/*
 * This program will help translate and calculate values for intergalatic transactions. 
 * It will begin by prompting you for an input data file which will contain the detailing notes for galatic units, transactions and questions. 
 * Program will accept relative or absolute filepath locations. Once loaded,
 * the program will parse each line and build dictionaries of questions, minerals and galatic units.
 * It will then attempt to calculate the result of each question using the dictionary of values and print the results to the screen.
 * 
*/
namespace GuideToTheGalaxy
{
    public class MerchantsGuideToTheGalaxy
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the Merchant's Guide to the Galaxy.");
            Console.WriteLine("Type a filename to read or type \"exit\" to quit the program.\n");

            while (true)
            {
                Console.Write(">");
                string input = Console.ReadLine();
                
                if (input == "exit")
                    break;

                try
                {
                    if (input.Contains(".txt") == false)
                        throw new Exception();

                    StreamReader file = new StreamReader(input);

                    string line;
                    var fileContentsCopy = new List<string>();

                    while ((line = file.ReadLine()) != null)
                    {
                        fileContentsCopy.Add(line);
                    }

                    file.Close();

                    MerchantsTradingGuide.Translate(fileContentsCopy.ToArray());
                    Console.WriteLine("\n");
                }
                catch
                {
                    Console.WriteLine("Bad file read.\n");
                }
            }
        }
    }
}
