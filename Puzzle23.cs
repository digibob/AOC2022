using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC2022
{
    using Elf23 = Tuple<int, int>;
    using Point23 = Tuple<int, int>;

    partial class Program
    {
        static void Puzzle23()
        {
            string[] lines = System.IO.File.ReadAllLines("puzzles/input23.txt");

            HashSet<Elf23> elves = new HashSet<Elf23>();

            for (int y = 0; y < lines.Length; y++)
            {
                string line = lines[y];

                for (int x = 0; x < line.Length; x++)
                {
                    if (line[x] == '#')
                    {
                        elves.Add(new Elf23(x, y));
                    }
                }
            }

            int start_facing = 0;

            bool IsFree(Elf23 elf)
            {
                return !elves.Contains(new Point23(elf.Item1 - 1, elf.Item2 - 1))
                    && !elves.Contains(new Point23(elf.Item1 - 1, elf.Item2))
                    && !elves.Contains(new Point23(elf.Item1 - 1, elf.Item2 + 1))
                    && !elves.Contains(new Point23(elf.Item1, elf.Item2 - 1))
                    && !elves.Contains(new Point23(elf.Item1, elf.Item2 + 1))
                    && !elves.Contains(new Point23(elf.Item1 + 1, elf.Item2 - 1))
                    && !elves.Contains(new Point23(elf.Item1 + 1, elf.Item2))
                    && !elves.Contains(new Point23(elf.Item1 + 1, elf.Item2 + 1));
            }

            Point23 GetFacingPoint(Elf23 elf, int facing)
            {
                switch (facing)
                {
                    case 0:
                        Point23 north = new Point23(elf.Item1, elf.Item2 - 1);
                        if (!elves.Contains(new Point23(north.Item1 - 1, north.Item2))
                            && !elves.Contains(north)
                            && !elves.Contains(new Point23(north.Item1 + 1, north.Item2)))
                        {
                            return north;
                        }
                        break;

                    case 1:
                        Point23 south = new Point23(elf.Item1, elf.Item2 + 1);
                        if (!elves.Contains(new Point23(south.Item1 - 1, south.Item2))
                            && !elves.Contains(south)
                            && !elves.Contains(new Point23(south.Item1 + 1, south.Item2)))
                        {
                            return south;
                        }
                        break;

                    case 2:
                        Point23 west = new Point23(elf.Item1 - 1, elf.Item2);
                        if (!elves.Contains(new Point23(west.Item1, west.Item2 - 1))
                            && !elves.Contains(west)
                            && !elves.Contains(new Point23(west.Item1, west.Item2 + 1)))
                        {
                            return west;
                        }
                        break;

                    case 3:
                        Point23 east = new Point23(elf.Item1 + 1, elf.Item2);
                        if (!elves.Contains(new Point23(east.Item1, east.Item2 - 1))
                            && !elves.Contains(east)
                            && !elves.Contains(new Point23(east.Item1, east.Item2 + 1)))
                        {
                            return east;
                        }
                        break;
                }

                return null;
            }

            for (int round = 0; round < 10000; round++, start_facing++)
            {
                Dictionary<Point23, int> target_counts = new Dictionary<Point23, int>();
                Dictionary<Elf23, Point23> elf_targets = new Dictionary<Elf23, Point23>();

                foreach (Elf23 elf in elves)
                {
                    if (IsFree(elf))
                    {
                        continue;
                    }

                    for (int f = 0; f < 4; f++)
                    {
                        Point23 point = GetFacingPoint(elf, (start_facing + f) % 4);
                        if (point != null)
                        {
                            elf_targets[elf] = point;
                            if (target_counts.ContainsKey(point))
                            {
                                target_counts[point]++;
                            }
                            else
                            {
                                target_counts.Add(point, 1);
                            }
                            break;
                        }
                    }
                }

                if (elf_targets.Count == 0)
                {
                    Console.WriteLine("{0}", round + 1);
                    break;
                }

                foreach (var KV in elf_targets)
                {
                    if (target_counts[KV.Value] != 1)
                    {
                        continue;
                    }

                    elves.Remove(KV.Key);
                    elves.Add(KV.Value);
                }
            }

            int min_x = elves.Min(e => e.Item1);
            int max_x = elves.Max(e => e.Item1);
            int min_y = elves.Min(e => e.Item2);
            int max_y = elves.Max(e => e.Item2);

            int empty_tiles = ((max_x - min_x + 1) * (max_y - min_y + 1)) - elves.Count;

            Console.WriteLine("{0}", empty_tiles);
        }
    }
}
