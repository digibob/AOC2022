using System;
using System.Linq;

namespace AOC2022
{
    partial class Program
    {
        struct Puzzle04Data
        {
            private readonly int[] range1;
            private readonly int[] range2;

            public Puzzle04Data(string line)
            {
                var entries = line.Split(',');
                range1 = entries[0].Split('-').Select(x => int.Parse(x)).ToArray();
                range2 = entries[1].Split('-').Select(x => int.Parse(x)).ToArray();
            }

            public bool ValuesAreContained()
            {
                return ((range1[0] >= range2[0]) && (range1[1] <= range2[1])) || 
                    ((range2[0] >= range1[0]) && (range2[1] <= range1[1]));
            }

            public bool ValuesOverlap()
            {
                return !((range1[1] < range2[0]) || (range1[0] > range2[1]));
            }
        }

        static void Puzzle04()
        {
            string[] lines = System.IO.File.ReadAllLines("puzzles/input04.txt");

            int count1 = 0;
            int count2 = 0;

            foreach (var data in lines.Select(x => new Puzzle04Data(x)))
            {
                if (data.ValuesAreContained())
                {
                    count1++;
                }

                if (data.ValuesOverlap())
                {
                    count2++;
                }
            }

            Console.WriteLine("{0}", count1);
            Console.WriteLine("{0}", count2);
        }
    }
}
