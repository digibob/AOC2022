using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC2022
{
    partial class Program
    {
        static void Puzzle20()
        {
            string[] lines = System.IO.File.ReadAllLines("puzzles/input20.txt");

            long Mod(long k, long n)
            {
                k %= n;
                return k < 0 ? k + n : k;
            }

            void Run(long multiplier, int num_rounds)
            {
                long[] numbers = lines
                    .Select(x => long.Parse(x) * multiplier)
                    .ToArray();

                List<int> indices = new List<int>();
                for (int index = 0; index < numbers.Length; index++)
                {
                    indices.Add(index);
                }

                int modulo = numbers.Length - 1;

                for (int round = 0; round < num_rounds; round++)
                {
                    for (int index = 0; index < numbers.Length; index++)
                    {
                        long number = numbers[index];
                        if (number == 0)
                        {
                            continue;
                        }

                        int start_index = indices.IndexOf(index);

                        int next_index = (int)Mod(start_index + number, modulo);

                        indices.RemoveAt(start_index);

                        if (next_index == 0)
                        {
                            indices.Add(index);
                        }
                        else
                        {
                            indices.Insert(next_index, index);
                        }
                    }
                }

                List<long> end_values = indices.Select(x => numbers[x]).ToList();

                int index_of_zero = end_values.IndexOf(0);
                int index_1000 = (index_of_zero + 1000) % numbers.Length;
                int index_2000 = (index_of_zero + 2000) % numbers.Length;
                int index_3000 = (index_of_zero + 3000) % numbers.Length;

                long result = end_values[index_1000] + end_values[index_2000] + end_values[index_3000];

                Console.WriteLine("{0}", result);
            }

            Run(1, 1);
            Run(811589153, 10);
        }
    }
}
