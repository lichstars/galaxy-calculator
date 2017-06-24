using System;

namespace GuideToTheGalaxy
{
    public class MerchantsTradingGuide
    {
        
        /* Parses the array of strings sent as a parameter and builds a dictionary of galaticUnits, minerals and questions 
         * depending on what each line of input contains.
         * Once this process is complete, calculators are called to solve any questions found.
         * Results are printed to the screen
         */
        public static void Translate(string[] input)
        {
            Context galaxy = new Context();

            GalaxyParser[] parsers = new GalaxyParser[] {
                    new GalaticUnitParser(galaxy),
                    new MineralParser(galaxy),
                    new QuestionParser(galaxy)
                };

            GalaxyCalculator[] calculators = new GalaxyCalculator[] {
                 new GalacticCalculator(galaxy),
                 new MineralsCalculator(galaxy)
            };

            foreach (string transaction in input)
            {
                foreach (GalaxyParser parser in parsers)
                {
                    if (parser.parse(transaction))
                        break;
                }
            }

            foreach (var question in galaxy.Questions)
            {
                bool calculated = false;
                foreach (GalaxyCalculator calculator in calculators)
                {
                    if (calculator.calculate(question))
                    {
                        calculated = true;
                        break;
                    }
                }
                if (!calculated)
                    Console.WriteLine("I have no idea what you are talking about");
            }
        }

    }
}
