using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC2022
{
    partial class Program
    {
        class Puzzle15Data
        {
            public int[] sensor_coords;
            public int[] beacon_coords;
        }

        static void Puzzle15()
        {
            string[] lines = System.IO.File.ReadAllLines("puzzles/input15.txt");

            List<Puzzle15Data> data_list = new List<Puzzle15Data>();

            foreach (string line in lines)
            {
                string[] entries = line[10..].Split(":");

                Puzzle15Data new_data = new Puzzle15Data
                {
                    sensor_coords = entries[0].Split(", ").Select(x => int.Parse(x[2..])).ToArray(),
                    beacon_coords = entries[1][22..].Split(", ").Select(x => int.Parse(x[2..])).ToArray()
                };
                data_list.Add(new_data);
            }

            {
                int target_y = 2000000;

                HashSet<int> entries = new HashSet<int>();

                foreach (Puzzle15Data data in data_list)
                {
                    int dist_x = Math.Abs(data.sensor_coords[0] - data.beacon_coords[0]);
                    int dist_y = Math.Abs(data.sensor_coords[1] - data.beacon_coords[1]);
                    int dist = dist_x + dist_y;

                    int target_dist = Math.Abs(data.sensor_coords[1] - target_y);

                    int width = dist - target_dist;

                    for (int x = data.sensor_coords[0] - width; x <= data.sensor_coords[0] + width; x++)
                    {
                        entries.Add(x);
                    }
                }

                foreach (Puzzle15Data data in data_list)
                {
                    if (data.beacon_coords[1] == target_y)
                    {
                        entries.Remove(data.beacon_coords[0]);
                    }
                }

                int min_x = entries.Min();
                int max_x = entries.Max();

                int num_blocked = 0;
                for (int x = min_x; x <= max_x; x++)
                {
                    if (entries.Contains(x))
                    {
                        num_blocked++;
                    }
                }

                Console.WriteLine("{0}", num_blocked);
            }

            int limit = 4000000;

            for (int target_y = 0; target_y <= limit; target_y++)
            {
                List<Tuple<int, int>> segments = new List<Tuple<int, int>>
                {
                    new Tuple<int, int>(0, limit + 1)
                };

                foreach (Puzzle15Data data in data_list)
                {
                    int dist_x = Math.Abs(data.sensor_coords[0] - data.beacon_coords[0]);
                    int dist_y = Math.Abs(data.sensor_coords[1] - data.beacon_coords[1]);
                    int dist = dist_x + dist_y;

                    int target_dist = Math.Abs(data.sensor_coords[1] - target_y);

                    int width = dist - target_dist;

                    if (width < 1)
                    {
                        continue;
                    }

                    int start = Math.Max(data.sensor_coords[0] - width, 0);
                    int end = Math.Min(data.sensor_coords[0] + width, limit);

                    List<Tuple<int, int>> new_segments = new List<Tuple<int, int>>();
                    foreach (Tuple<int, int> s in segments)
                    {
                        if (start > s.Item2 || end < s.Item1)
                        {
                            new_segments.Add(s);
                            continue;
                        }

                        int left_len = (start - s.Item1);
                        if (left_len > 0)
                        {
                            new_segments.Add(new Tuple<int, int>(s.Item1, s.Item1 + left_len - 1));
                        }

                        int right_len = (s.Item2 - end);
                        if (right_len > 0)
                        {
                            new_segments.Add(new Tuple<int, int>(end + 1, end + right_len - 1));
                        }
                    }
                    segments = new_segments;
                }

                if (segments.Count > 0)
                {
                    Console.WriteLine("{1} {0}", target_y, segments[0].Item1);
                    Console.WriteLine("{0}", ((Int64)segments[0].Item1 * (Int64)limit) + (Int64)target_y);
                    break;
                }
            }
        }
    }
}
