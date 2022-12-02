using System;
using System.Linq;

namespace AOC2022
{
    class Program
    {
        static void Main(string[] args)
        {
            Puzzle01();
            //Puzzle02();
        }

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
        static void Puzzle02()
        {
            string[] lines = System.IO.File.ReadAllLines("puzzles/input02.txt");

            int total_score1 = 0;
            int total_score2 = 0;
            foreach (string line in lines)
            {
                int opponent_item = line[0] - 'A';
                int my_value = line[2] - 'X';

                int round_score1 = ((4 + (my_value - opponent_item)) % 3) * 3;
                total_score1 += (my_value + 1) + round_score1;

                int item_round2 = (opponent_item + my_value + 2) % 3;
                total_score2 += (my_value * 3) + (item_round2 + 1);
            }

            Console.WriteLine("{0}", total_score1);
            Console.WriteLine("{0}", total_score2);
        }
    }
}
