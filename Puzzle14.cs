using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC2022
{
    partial class Program
    {
        static void Puzzle14()
        {
            string[] lines = System.IO.File.ReadAllLines("puzzles/input14.txt");

            Dictionary<Tuple<int, int>, char> items = new Dictionary<Tuple<int, int>, char>();

            foreach (string line in lines)
            {
                string[] point_strings = line.Split(" -> ");

                int[] first_point_data = point_strings[0].Split(",").Select(x => int.Parse(x)).ToArray();
                Tuple<int, int> prev_point = new Tuple<int, int>(first_point_data[0], first_point_data[1]);

                foreach (string point_string in point_strings.Skip(1))
                {
                    int[] point_data = point_string.Split(",").Select(x => int.Parse(x)).ToArray();
                    Tuple<int, int> point = new Tuple<int, int>(point_data[0], point_data[1]);

                    if (point.Item1 == prev_point.Item1)
                    {
                        int start = Math.Min(prev_point.Item2, point.Item2);
                        int end = Math.Max(prev_point.Item2, point.Item2) + 1;

                        for (int y = start; y < end; y++)
                        {
                            items.TryAdd(new Tuple<int, int>(point.Item1, y), '#');
                        }
                    }
                    else
                    {
                        int start = Math.Min(prev_point.Item1, point.Item1);
                        int end = Math.Max(prev_point.Item1, point.Item1) + 1;

                        for (int x = start; x < end; x++)
                        {
                            items.TryAdd(new Tuple<int, int>(x, point.Item2), '#');
                        }
                    }

                    prev_point = point;
                }
            }

            int max_y_sim = items.Max(x => x.Key.Item2);

            int num_sand = 0;
            int? num_sand_into_void = null;

            while (true)
            {
                int sand_x = 500;
                int sand_y = 0;

                if (items.ContainsKey(new Tuple<int, int>(sand_x, sand_y)))
                {
                    break;
                }

                while (sand_y <= max_y_sim)
                {
                    if (!items.ContainsKey(new Tuple<int, int>(sand_x, sand_y + 1)))
                    {
                        sand_y += 1;
                        continue;
                    }

                    if (!items.ContainsKey(new Tuple<int, int>(sand_x - 1, sand_y + 1)))
                    {
                        sand_x -= 1;
                        sand_y += 1;
                        continue;
                    }

                    if (!items.ContainsKey(new Tuple<int, int>(sand_x + 1, sand_y + 1)))
                    {
                        sand_x += 1;
                        sand_y += 1;
                        continue;
                    }

                    items.Add(new Tuple<int, int>(sand_x, sand_y), 'o');
                    num_sand++;
                    break;
                }

                if (sand_y > max_y_sim)
                {
                    if (!num_sand_into_void.HasValue)
                    {
                        num_sand_into_void = num_sand;
                    }

                    items.Add(new Tuple<int, int>(sand_x, sand_y), 'o');
                    num_sand++;
                }
            }

            //int min_x = items.Min(x => x.Key.Item1);
            //int max_x = items.Max(x => x.Key.Item1);

            //int min_y = items.Min(x => x.Key.Item2);
            //int max_y = items.Max(x => x.Key.Item2);

            //for (int y = min_y; y <= max_y; y++)
            //{
            //    string line = string.Empty;

            //    for (int x = min_x; x <= max_x; x++)
            //    {
            //        char value;
            //        if (!items.TryGetValue(new Tuple<int, int>(x, y), out value))
            //        {
            //            value = '.';
            //        }

            //        line += value;
            //    }

            //    Console.WriteLine("{0}", line);
            //}
            
            Console.WriteLine("{0}", num_sand_into_void.Value);
            Console.WriteLine("{0}", num_sand);
        }
    }
}
