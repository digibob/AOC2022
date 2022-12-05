using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC2022
{
    partial class Program
    {
        static void Puzzle05()
        {
            int num_columns = 9;
            int num_rows = 8;

            string[] lines = System.IO.File.ReadAllLines("puzzles/input05.txt");

            Stack<char>[] stacks1 = new Stack<char>[num_columns];
            Stack<char>[] stacks2 = new Stack<char>[num_columns];
            Stack<char> temp_stack = new Stack<char>();

            for (int column_index = 0; column_index < num_columns; column_index++)
            {
                stacks1[column_index] = new Stack<char>();
                stacks2[column_index] = new Stack<char>();
            }

            for (int row_index = num_rows - 1; row_index >= 0; row_index--)
            {
                string row = lines[row_index];

                for (int column_index = 0; column_index < num_columns; column_index++)
                {
                    int character_index = (column_index * 4) + 1;
                    char crate = row[character_index];
                    if (crate == ' ')
                    {
                        continue;
                    }

                    stacks1[column_index].Push(crate);
                    stacks2[column_index].Push(crate);
                }
            }

            foreach (string line in lines.Skip(10))
            {
                string clean_line = line.Replace("move", "");
                string[] first_set = clean_line.Split("from");
                string[] second_set = first_set[1].Split("to");

                int crate_count = int.Parse(first_set[0]);
                int from_column = int.Parse(second_set[0]) - 1;
                int to_column = int.Parse(second_set[1]) - 1;

                for (int crate_index = 0; crate_index < crate_count; crate_index++)
                {
                    stacks1[to_column].Push(stacks1[from_column].Pop());
                    temp_stack.Push(stacks2[from_column].Pop());
                }

                while (temp_stack.Count > 0)
                {
                    stacks2[to_column].Push(temp_stack.Pop());
                }
            }

            string final_string1 = string.Empty;
            string final_string2 = string.Empty;
            for (int column_index = 0; column_index < num_columns; column_index++)
            {
                final_string1 += stacks1[column_index].Pop();
                final_string2 += stacks2[column_index].Pop();
            }

            Console.WriteLine("{0}", final_string1);
            Console.WriteLine("{0}", final_string2);
        }
    }
}
