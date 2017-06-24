using System;
using System.Collections.Generic;
using System.Linq;

namespace GuideToTheGalaxy
{
    class Context
    {
        public List<string> Questions { get; set; }
        public Dictionary<string, RomanNumeral> GalacticUnits { get; set; }
        public Dictionary<string, double> Minerals { get; set; }

        public List<RomanNumeral> RomanNumeral = new List<RomanNumeral> {
                new RomanNumeral('I', 1, true),
                new RomanNumeral('V', 5, false),
                new RomanNumeral('X', 10, true),
                new RomanNumeral('L', 50, false),
                new RomanNumeral('C', 100, true),
                new RomanNumeral('D', 500, false),
                new RomanNumeral('M', 1000, true)               
            };

        public Context()
        {
            Questions = new List<string>();
            Minerals = new Dictionary<string, double>();
            GalacticUnits = new Dictionary<string, RomanNumeral>();
        }
    }

    abstract class GalaxyParser
    {
        public Context Galaxy { get; set; }

        public GalaxyParser(Context galaxy)
        {
            Galaxy = galaxy;
        }

        public abstract bool parse(string input);
    }

    /* Parses strings that contain the 'is' keyword and must ONLY have 1 string on either side of the keyword 'is' (eg. "glob is I" ).
     * This definition is  broken down and stored into a GalacticUnits dictionary with a key of the galatic Unit (e.g "glob") and 
     * its corresponding roman numeral object (e.g. "I") 
     */
    class GalaticUnitParser : GalaxyParser
    {
        public GalaticUnitParser(Context galaxy) : base(galaxy) { }

        public override bool parse(string input)
        {
            string[] keywords = input.Split(new[] { " is " }, StringSplitOptions.RemoveEmptyEntries);

            if (keywords.Count() != 2)
                return false;
            if (keywords[1].Trim().Length > 1)
                return false;
            if (keywords[0].Trim().Length < 1)
                return false;
            if (keywords[1] == null)
                return false;

            string galaticUnit = keywords[0].Trim();

            char symbol = keywords[1][0];

            RomanNumeral romanNumeral = Galaxy.RomanNumeral.Find(r => r.Symbol == symbol);

            if (romanNumeral == null)
                return false;

            Galaxy.GalacticUnits.Add(galaticUnit, romanNumeral);

            return true;
        }
    }

    /* Parses strings that contain mineral elements such as "glob glob Silver is 34 Credits" 
     * by breaking up the string with the keyword 'is' and parsing the first half by assuming the last keyword being a mineral and the parsing any other keywords
     * as galatic units. We then parse the second half of the input string by assuming the numerical value is the first text in the array.
     * Using the following formula to then calculate the value of the mineral = credits / galatic units value (eg. Silver = 34 / glob glob)
     * A Minerals dictionary matching the galatic Mineral (e.g "Silver") with its calculated value (e.g. 17) is updated.
     */
    class MineralParser : GalaxyParser
    {
        public MineralParser(Context galaxy) : base(galaxy) { }

        public override bool parse(string input)
        {
            int totalCredits;
            
            string[] components = input.Split(new[] { " is " }, StringSplitOptions.RemoveEmptyEntries);

            //components = glob glob Silver //  34 credits
            if (components.Count() != 2)
                return false;

            string[] keywords = components[0].Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            if (keywords.Count() < 1)
                return false;

            var mineral = keywords.Last();

            var galaticWords = keywords.Take(keywords.Count() - 1).ToArray();

            var galaticValue = GalaticUnitsCalculator.calculate(galaticWords, Galaxy.GalacticUnits);

            if (galaticValue == -1)
                return false;

            var creditKeyword = components[1].Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            bool isNumber = Int32.TryParse(creditKeyword[0].ToString(), out totalCredits);
            if (!isNumber)
                return false;

            double mineralValue = totalCredits / galaticValue;

            Galaxy.Minerals.Add(mineral, mineralValue);

            return true;
        }
    }

    /* Parses strings that contain a question mark denoting a question being asked. These are added to the Question dictionary.*/
    class QuestionParser : GalaxyParser
    {
        public QuestionParser(Context galaxy) : base(galaxy) { }

        public override bool parse(string input)
        {
            if (input.EndsWith("?") == false)
                return false;

            Galaxy.Questions.Add(input);

            return true;
        }
    }
}

