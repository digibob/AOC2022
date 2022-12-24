using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC2022
{
    partial class Program
    {
        static void Puzzle18()
        {
            HashSet<Tuple<int, int, int>> points = System.IO.File.ReadAllLines("puzzles/input18.txt")
                .Select(x => x.Split(",").Select(x => int.Parse(x)).ToArray())
                .Select(x => new Tuple<int, int, int>(x[0], x[1], x[2]))
                .ToHashSet();

            bool OffsetIsLava(Tuple<int, int, int> point, int offset_x, int offset_y, int offset_z)
            {
                return points.Contains(new Tuple<int, int, int>(point.Item1 + offset_x, point.Item2 + offset_y, point.Item3 + offset_z));
            }

            HashSet<Tuple<int, int, int>> outside_points = new HashSet<Tuple<int, int, int>>();

            bool OffsetIsOutside(Tuple<int, int, int> point, int offset_x, int offset_y, int offset_z)
            {
                return outside_points.Contains(new Tuple<int, int, int>(point.Item1 + offset_x, point.Item2 + offset_y, point.Item3 + offset_z));
            }

            int min_x = points.Min(x => x.Item1);
            int max_x = points.Max(x => x.Item1);
            int min_y = points.Min(x => x.Item2);
            int max_y = points.Max(x => x.Item2);
            int min_z = points.Min(x => x.Item3);
            int max_z = points.Max(x => x.Item3);

            bool OutsideIsReachable(Tuple<int, int, int> point)
            {
                if (point.Item1 == min_x || point.Item1 == max_x
                    || point.Item2 == min_y || point.Item2 == max_y
                    || point.Item3 == min_z || point.Item3 == max_z)
                {
                    return true;
                }

                if (OffsetIsOutside(point, 0, 0, -1) ||
                    OffsetIsOutside(point, 0, 0, 1) ||
                    OffsetIsOutside(point, 0, -1, 0) ||
                    OffsetIsOutside(point, 0, 1, 0) ||
                    OffsetIsOutside(point, -1, 0, 0) ||
                    OffsetIsOutside(point, 1, 0, 0))
                {
                    return true;
                }

                return false;
            }

            bool changed = true;
            while (changed)
            {
                changed = false;

                for (int x = min_x; x <= max_x; x++)
                {
                    for (int y = min_y; y <= max_y; y++)
                    {
                        for (int z = min_z; z <= max_z; z++)
                        {
                            Tuple<int, int, int> point = new Tuple<int, int, int>(x, y, z);

                            if (points.Contains(point) || outside_points.Contains(point))
                            {
                                continue;
                            }

                            if (OutsideIsReachable(point))
                            {
                                outside_points.Add(point);
                                changed = true;
                            }
                        }
                    }
                }
            }

            int CountSides()
            {
                int sides = 0;
                foreach (Tuple<int, int, int> point in points)
                {
                    if (!OffsetIsLava(point, 0, 0, -1))
                    {
                        sides++;
                    }

                    if (!OffsetIsLava(point, 0, 0, 1))
                    {
                        sides++;
                    }

                    if (!OffsetIsLava(point, 0, -1, 0))
                    {
                        sides++;
                    }

                    if (!OffsetIsLava(point, 0, 1, 0))
                    {
                        sides++;
                    }

                    if (!OffsetIsLava(point, -1, 0, 0))
                    {
                        sides++;
                    }

                    if (!OffsetIsLava(point, 1, 0, 0))
                    {
                        sides++;
                    }
                }

                return sides;
            }

            int sides1 = CountSides();

            for (int x = min_x; x <= max_x; x++)
            {
                for (int y = min_y; y <= max_y; y++)
                {
                    for (int z = min_z; z <= max_z; z++)
                    {
                        Tuple<int, int, int> point = new Tuple<int, int, int>(x, y, z);

                        if (points.Contains(point) || outside_points.Contains(point))
                        {
                            continue;
                        }

                        points.Add(point);
                    }
                }
            }

            int sides2 = CountSides();

            Console.WriteLine("{0}", sides1);
            Console.WriteLine("{0}", sides2);
        }
    }
}
