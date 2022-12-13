using System;
using System.Collections.Generic;

namespace AOC2022
{
    partial class Program
    {
        class Puzzle13List
        {
            static int FindCloseIndex(string content, int start_index)
            {
                int num_brackets = 1;

                for (int index = start_index + 1; index < content.Length; index++)
                {
                    if (content[index] == '[')
                    {
                        num_brackets++;
                    }
                    else if (content[index] == ']')
                    {
                        num_brackets--;

                        if (num_brackets == 0)
                        {
                            return index;
                        }
                    }
                }
                
                return -1;
            }
            public Puzzle13List(int value)
            {
                entries.Add(value);
            }

            public Puzzle13List(string line)
            {
                string content = line.Substring(1, line.Length - 2);

                for (int index = 0; index < content.Length; index++)
                {
                    if (char.IsNumber(content[index]))
                    {
                        int start_index = index;
                        index++;
                        while (index < content.Length && char.IsNumber(content[index]))
                        {
                            index++;
                        }

                        string sub_string = content.Substring(start_index, index - start_index);
                        int value = int.Parse(sub_string);
                        entries.Add(value);
                    }
                    else if (content[index] == '[')
                    {
                        int end_index = FindCloseIndex(content, index);

                        var entry = new Puzzle13List(content.Substring(index, end_index - index + 1));
                        entries.Add(entry);

                        index = end_index + 1;
                    }
                }
            }

            public readonly List<object> entries = new List<object>();
        }

        static int OrderLists(object left, object right)
        {
            if (left is int && right is int)
            {
                return Math.Clamp((int)left - (int)right, -1, 1);
            }

            if (left is Puzzle13List && right is Puzzle13List)
            {
                Puzzle13List left_node = left as Puzzle13List;
                Puzzle13List right_node = right as Puzzle13List;

                int limit = Math.Max(left_node.entries.Count, right_node.entries.Count);
                for (int entry_index = 0; entry_index < limit; entry_index++)
                {
                    if (entry_index >= left_node.entries.Count)
                    {
                        return -1;
                    }
                    if (entry_index >= right_node.entries.Count)
                    {
                        return 1;
                    }

                    int value = OrderLists(left_node.entries[entry_index], right_node.entries[entry_index]);
                    if (value == 0)
                    {
                        continue;
                    }

                    return value;
                }

                return 0;
            }

            if (left is Puzzle13List)
            {
                return OrderLists(left, new Puzzle13List((int)right));
            }
            else
            {
                return OrderLists(new Puzzle13List((int)left), right);
            }
        }

        static void Puzzle13()
        {
            string[] lines = System.IO.File.ReadAllLines("puzzles/input13.txt");

            List<Puzzle13List> all_lists = new List<Puzzle13List>();

            int sum = 0;
            for (int pair_index = 0; pair_index < lines.Length; pair_index += 3)
            {
                Puzzle13List list1 = new Puzzle13List(lines[pair_index]);
                Puzzle13List list2 = new Puzzle13List(lines[pair_index + 1]);

                int result = OrderLists(list1, list2);
                if (result == -1)
                {
                    sum += (pair_index / 3) + 1;
                }

                all_lists.Add(list1);
                all_lists.Add(list2);
            }

            var key1 = new Puzzle13List("[[2]]");
            var key2 = new Puzzle13List("[[6]]");

            all_lists.Add(key1);
            all_lists.Add(key2);

            all_lists.Sort(OrderLists);

            int index1 = all_lists.IndexOf(key1) + 1;
            int index2 = all_lists.IndexOf(key2) + 1;

            Console.WriteLine("{0}", sum);
            Console.WriteLine("{0}", index1 * index2);
        }
    }
}
