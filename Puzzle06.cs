using System;

namespace AOC2022
{
    partial class Program
    {
        static void Puzzle06()
        {
            string line = System.IO.File.ReadAllText("puzzles/input06.txt");

            Action<int> test_func = (int count) =>
            {
                var set = new System.Collections.Generic.HashSet<char>();

                for (int start_index = 0; start_index < line.Length - (count - 1); start_index++)
                {
                    for (int index = start_index; index < start_index + count; index++)
                    {
                        set.Add(line[index]);
                    }

                    if (set.Count == count)
                    {
                        Console.WriteLine("{0}", start_index + count);
                        break;
                    }

                    set.Clear();
                }
            };

            test_func(4);
            test_func(14);
        }
    }
}
