using System.Collections.Generic;
using System.Linq;

namespace GuideToTheGalaxy
{
    /* Responsible for calculating the numerical value of a string of galatic units when passed a dictionary of parsed galatic units
     * The caculate function will take a string of galatic keywords. This will be converted into respective roman numerals using the 
     * galatic units dictionary. If the corresponding roman numeral is not found for a galatic keyword, -1 is returned indicating an error.
     * Repeating roman numerals is also checked. Those that can be repeated must not be repeated 3 times in successsion. -1 is returned if an
     * error is found. Once validation complete, the calculation of roman numerals into a numerical value is performed.
    */
    public static class GalaticUnitsCalculator
    {
        /*Check that galatic units have a definition in the dictionary and if so build a list of the roman numerals and return it
         Taking an input of 'glob prok' and a dictionary of galatic units translated*/
        static List<RomanNumeral> getRomanNumeral(string[] input, Dictionary<string, RomanNumeral> galaticUnits)
        {
            List<RomanNumeral> RomanNumeral = new List<RomanNumeral>();

            if (input.Count() <= 0)
                return null;

            foreach (var galaticUnit in input)
            {
                if (galaticUnits.ContainsKey(galaticUnit) == false)
                    return null;

                RomanNumeral.Add(galaticUnits[galaticUnit]);
            }
            return RomanNumeral;
        }

        /*Check that units that can be repeated are only repeated 3 times in succession*/
        static bool validateRepeatedUnits(string[] input, Dictionary<string, RomanNumeral> galaticUnits, List<RomanNumeral> RomanNumeral)
        {
            for (int i = 0; i < RomanNumeral.Count(); i++)
            {
                if (i + 1 == RomanNumeral.Count())
                    break;
                if (RomanNumeral[i].Value == RomanNumeral[i + 1].Value)
                {
                    if (RomanNumeral[i].CanRepeat == false)
                        return false;


                    int repeatCount = 1;
                    for (int j = i; j < RomanNumeral.Count(); j++)
                    {
                        if (j + 1 == RomanNumeral.Count)
                            break;
                        if (RomanNumeral[j].Symbol != RomanNumeral[j + 1].Symbol)
                            break;

                        repeatCount++;
                        if (repeatCount > 3)
                            return false;
                    }
                }
            }
            return true;
        }

        /*calculate the value for a given input string contain galatic units (eg. glob prob) by converting them into roman numerals 
         and calculating the value of those.*/
        public static double calculate(string[] input, Dictionary<string, RomanNumeral> galaticUnits)
        {
            List<RomanNumeral> romanNumeral = new List<RomanNumeral>();
            double value = 0;

            romanNumeral = getRomanNumeral(input, galaticUnits);

            if (romanNumeral == null)
                return -1;

            if (validateRepeatedUnits(input, galaticUnits, romanNumeral) == false)
                return -1;

            for (int i = 0; i < romanNumeral.Count(); i++)
            {
                var currentUnit = romanNumeral[i];

                value += currentUnit.Value;

                if (i + 1 > romanNumeral.Count() - 1)
                    return value;

                var nextUnit = romanNumeral[i + 1];

                //when smaller values precede larger values, we subtract larger - smaller
                if (currentUnit.Value < nextUnit.Value)
                {
                    value = nextUnit.Value - value;
                    i++;
                }
            }

            return value;
        }
    }

}
