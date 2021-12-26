using System;
using System.IO;
using System.Text.RegularExpressions;

namespace com.fabioscagliola.AdventOfCode202108
{
    class Program
    {
        static void Main()
        {
            // I manually split the input in two files using the | as a separator 

            // PART 1 

            Regex input1Regex = new Regex(@"\b(?:[abcdefg]{2}|[abcdefg]{3}|[abcdefg]{4}|[abcdefg]{7})\b");
            MatchCollection matchCollection = input1Regex.Matches(File.ReadAllText("Input2.txt"));
            Console.WriteLine($"In the output values, digits 1, 4, 7, or 8 appear {matchCollection.Count} times");

            // TODO: PART 2 

        }

    }
}

