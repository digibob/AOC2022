using System;

namespace AOC2022
{
    partial class Program
    {
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
