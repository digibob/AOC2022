using System;
using System.Linq;

namespace AOC2022
{
    partial class Program
    {
        static void Puzzle01()
        {
            int[] values = System.IO.File.ReadAllLines("puzzles/input01.txt")
                .Select(x => { int value = 0; int.TryParse(x, out value); return value; })
                .ToArray();

            var elves = new System.Collections.Generic.List<int>();

            int max = 0;
            int count = 0;
            foreach (int value in values)
            {
                if (value == 0)
                {
                    elves.Add(count);
                    count = 0;
                }
                else
                {
                    count += value;
                    max = Math.Max(count, max);
                }
            }

            int total = elves.OrderByDescending(x => x).Take(3).Sum();

            Console.WriteLine("{0}", max);
            Console.WriteLine("{0}", total);
        }
    }
}
