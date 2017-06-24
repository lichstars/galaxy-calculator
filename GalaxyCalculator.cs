using System;
using System.Collections.Generic;
using System.Linq;

namespace GuideToTheGalaxy
{
    /*class to calculate questions in the form of 'how many Credits .. ?' or 'how much is ... ?' */
    abstract class GalaxyCalculator
    {
        public Context Galaxy { get; set; }

        public GalaxyCalculator(Context galaxy)
        {
            Galaxy = galaxy;
        }

        public abstract bool calculate(string input);
    }

    /* MineralsCalculator will answer questions that involve a mineral in the question (e.g. how many Credits is glob prok Silver ?) 
     * This is determined by checking if the string contains the keyword 'Credits' in the question.
     * The calculator will split the string by the keyword 'is' then further split the remaining keywords by '  ' and focusing mainly on the 
     * second half of the question (e.g. glob prok Silver). Referencing the minerals dictionary and the galatic units calculator.
     * A string is printed to the console with the answer to the question.
     */
    class MineralsCalculator : GalaxyCalculator
    {
        public MineralsCalculator(Context galaxy) : base(galaxy) { }

        public override bool calculate(string input)
        {
            if (input.Contains("Credits") == false)
                return false;

            string[] components = input.Split(new[] { " is " }, StringSplitOptions.RemoveEmptyEntries);

            if (components.Count() != 2)
                return false;

            string[] keywords = components[1].Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            keywords = keywords.Take(keywords.Count() - 1).ToArray(); //remove "?"

            var mineral = keywords.Last();

            if (Galaxy.Minerals.ContainsKey(mineral) == false)
                return false;

            double mineralValue = Galaxy.Minerals[mineral];

            var galaticUnits = keywords.Take(keywords.Count() - 1).ToArray();

            double galaticValue = GalaticUnitsCalculator.calculate(galaticUnits, Galaxy.GalacticUnits);

            if (galaticValue == -1)
                return false;

            var answer = galaticValue * mineralValue;

            Console.WriteLine("{0} is {1} Credits", string.Join(" ", keywords), answer);

            return true;
        }
    }

    /* GalacticCalculator will answer questions in the form of (e.g. how much is glob glob prok ?) 
     * This is determined by checking if the string is absent of the keyword 'Credits' in the question.
     * The calculator will split the string by the keyword 'is' then further split the remaining keywords by '  ' and focusing mainly on the 
     * second half of the question (e.g. glob glob prok). Sending this array to the galactic units calculator to dertermine the numerical value.
     * A string is printed to the console with the answer to the question.
     */
    class GalacticCalculator : GalaxyCalculator
    {
        public GalacticCalculator(Context galaxy) : base(galaxy) { }

        public override bool calculate(string input)
        {
           if (input.Contains("Credits"))
                return false;

            string[] components = input.Split(new[] { " is " }, StringSplitOptions.RemoveEmptyEntries);

            if (components.Count() != 2)
                return false;

            string[] keywords = components[1].Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            var galaticUnits = keywords.Take(keywords.Count() - 1).ToArray(); //remove "?"

            var galaticValue = GalaticUnitsCalculator.calculate(galaticUnits, Galaxy.GalacticUnits);

            if (galaticValue == -1)
                return false;

            Console.WriteLine("{0} is {1}", string.Join(" ", galaticUnits), galaticValue);

            return true;
        }
    }
}
