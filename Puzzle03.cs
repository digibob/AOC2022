using System;
using System.Linq;

namespace AOC2022
{
    partial class Program
    {
        static void Puzzle03()
        {
            string[] lines = System.IO.File.ReadAllLines("puzzles/input03.txt");

            System.Func<char, int> score_func = (char x) => char.IsUpper(x) ? (x - 38) : (x - 96);

            int score = 0;

            foreach (string line in lines)
            {
                int mid = line.Length / 2;
                var left = line.Substring(0, mid).ToCharArray();
                var right = line.Substring(mid).ToCharArray();

                char shared = left.Intersect(right).First();

                score += score_func(shared);
            }

            Console.WriteLine("{0}", score);

            int score2 = 0;

            for (int index = 0; index < lines.Length; index += 3)
            {
                char shared = lines[index].Intersect(lines[index + 1]).Intersect(lines[index + 2]).First();

                score2 += score_func(shared);
            }

            Console.WriteLine("{0}", score2);
        }
    }
}
