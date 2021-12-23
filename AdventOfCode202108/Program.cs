using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace com.fabioscagliola.AdventOfCode202108
{
    static class Extensions
    {
        public static string Sort(this string s)
        {
            List<char> charList = s.ToList();
            charList.Sort();
            return new string(charList.ToArray());
        }

    }

    class Program
    {
        class SevenSegmentDisplay
        {
            protected char a;
            protected char b;
            protected char c;
            protected char d;
            protected char e;
            protected char f;
            protected char g;

            protected List<char> charList => new List<char> { a, b, c, d, e, f, g };

            public string Digit0 => new string(new char[] { a, b, c, d, e, g }).Sort();
            public string Digit1 => new string(new char[] { b, c }).Sort();
            public string Digit2 => new string(new char[] { a, b, d, e, g }).Sort();
            public string Digit3 => new string(new char[] { a, b, c, d, g }).Sort();
            public string Digit4 => new string(new char[] { b, c, f, g }).Sort();
            public string Digit5 => new string(new char[] { a, c, d, f, g }).Sort();
            public string Digit6 => new string(new char[] { a, c, d, e, f, g }).Sort();
            public string Digit7 => new string(new char[] { a, b, c }).Sort();
            public string Digit8 => new string(new char[] { a, b, c, d, e, f, g }).Sort();
            public string Digit9 => new string(new char[] { a, b, c, d, f, g }).Sort();

            protected List<string> DigitsList => new List<string> { Digit0, Digit1, Digit2, Digit3, Digit4, Digit5, Digit6, Digit7, Digit8, Digit9 };

            public SevenSegmentDisplay(string digit1, string digit4, string digit7, string digit8, List<string> digitList5CharLong, List<string> digitList6CharLong)
            {
                char[] chars1 = digit1.Sort().ToCharArray();
                char[] chars4 = digit4.Sort().ToCharArray();
                char[] chars7 = digit7.Sort().ToCharArray();
                char[] chars8 = digit8.Sort().ToCharArray();

                b = chars1[0];
                c = chars1[1];

                a = digit7.ToList().Find(x => !charList.Contains(x));

                f = digit4.ToList().Find(x => !charList.Contains(x));
                g = digit4.ToList().Find(x => !charList.Contains(x));

                d = digit8.ToList().Find(x => !charList.Contains(x));
                e = digit8.ToList().Find(x => !charList.Contains(x));

                if (digitList5CharLong.Count != digitList5CharLong.FindAll(x => x.Contains(g)).Count || digitList6CharLong.Count != digitList6CharLong.FindAll(x => x.Contains(f)).Count)
                {
                    char temp = f;
                    f = g;
                    g = temp;
                }

                if (digitList5CharLong.Count != digitList5CharLong.FindAll(x => x.Contains(d)).Count || digitList6CharLong.Count != digitList6CharLong.FindAll(x => x.Contains(d)).Count)
                {
                    char temp = d;
                    d = e;
                    e = temp;
                }
            }

            public int GetValue(List<string> digitsList)
            {
                StringBuilder stringBuilder = new StringBuilder();

                foreach (string digits in digitsList)
                    stringBuilder.Append(DigitsList.FindIndex(x => x == digits.Sort()));

                return int.Parse(stringBuilder.ToString());
            }

        }

        static void Main()
        {
            // I manually split the input in two files using the | as a separator 

            // PART 1 

            {
                Regex input1Regex = new Regex(@"\b(?:[abcdefg]{2}|[abcdefg]{3}|[abcdefg]{4}|[abcdefg]{7})\b");
                MatchCollection matchCollection = input1Regex.Matches(File.ReadAllText("Input2.txt"));
                Console.WriteLine($"In the output values, digits 1, 4, 7, or 8 appear {matchCollection.Count} times");
            }

            // PART 2 

            {
                Regex input1Regex = new Regex(@"\b(?:[abcdefg]{2}|[abcdefg]{3}|[abcdefg]{4}|[abcdefg]{5}|[abcdefg]{6}|[abcdefg]{7})\b");

                int result = 0;

                string[] input1 = File.ReadAllLines("Input1.txt");
                string[] input2 = File.ReadAllLines("Input2.txt");

                for (int i = 0; i < input1.Length; i++)
                {
                    MatchCollection input1MatchCollection = input1Regex.Matches(input1[i]);

                    string digit1 = null;
                    string digit4 = null;
                    string digit7 = null;
                    string digit8 = null;

                    List<string> digitList5CharLong = new List<string>();
                    List<string> digitList6CharLong = new List<string>();

                    foreach (Match match in input1MatchCollection)
                    {
                        switch (match.Value.Length)
                        {
                            case 2:
                                if (digit1 != null && digit1 != match.Value.Sort())
                                    throw new ApplicationException("Inconsistent inputs for digit 1 were found!");
                                digit1 = match.Value.Sort();
                                break;
                            case 3:
                                if (digit7 != null && digit7 != match.Value.Sort())
                                    throw new ApplicationException("Inconsistent inputs for digit 7 were found!");
                                digit7 = match.Value.Sort();
                                break;
                            case 4:
                                if (digit4 != null && digit4 != match.Value.Sort())
                                    throw new ApplicationException("Inconsistent inputs for digit 4 were found!");
                                digit4 = match.Value.Sort();
                                break;
                            case 5:
                                digitList5CharLong.Add(match.Value.Sort());
                                break;
                            case 6:
                                digitList6CharLong.Add(match.Value.Sort());
                                break;
                            case 7:
                                if (digit8 != null && digit8 != match.Value.Sort())
                                    throw new ApplicationException("Inconsistent inputs for digit 8 were found!");
                                digit8 = match.Value.Sort();
                                break;
                        }
                    }

                    SevenSegmentDisplay sevenSegmentDisplay = new SevenSegmentDisplay(digit1, digit4, digit7, digit8, digitList5CharLong, digitList6CharLong);

                    Regex input2Regex = new Regex($@"\b(?:[{sevenSegmentDisplay.Digit0}]{{6}})\b|\b(?:[{sevenSegmentDisplay.Digit1}]{{2}})\b|\b(?:[{sevenSegmentDisplay.Digit2}]{{5}})\b|\b(?:[{sevenSegmentDisplay.Digit3}]{{5}})\b|\b(?:[{sevenSegmentDisplay.Digit4}]{{4}})\b|\b(?:[{sevenSegmentDisplay.Digit5}]{{5}})\b|\b(?:[{sevenSegmentDisplay.Digit6}]{{6}})\b|\b(?:[{sevenSegmentDisplay.Digit7}]{{3}})\b|\b(?:[{sevenSegmentDisplay.Digit8}]{{7}})\b|\b(?:[{sevenSegmentDisplay.Digit9}]{{6}})\b");

                    MatchCollection input2MatchCollection = input2Regex.Matches(input2[i]);

                        List<string> digitsList = new List<string>();

                        foreach (Match match in input2MatchCollection)
                        {
                            digitsList.Add(match.Value);
                        }

                        result += sevenSegmentDisplay.GetValue(digitsList);
                }

                Console.WriteLine($"If I add up all of the output values, I get {result}");
            }
        }

    }
}

