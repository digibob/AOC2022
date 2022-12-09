using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC2022
{
    partial class Program
    {
        class Knot
        {
            public void Move(int move_x, int move_y)
            {
                pos_x += move_x;
                pos_y += move_y;

                if (child != null)
                {
                    child.Update(this);
                }
            }

            public void Update(Knot parent)
            {
                int move_x = pos_x - parent.pos_x;
                int move_y = pos_y - parent.pos_y;
                int move_x_abs = Math.Abs(move_x);
                int move_y_abs = Math.Abs(move_y);

                if ((move_x_abs > 1) || (move_y_abs > 1))
                {
                    pos_x -= Math.Clamp(move_x, -1, 1);
                    pos_y -= Math.Clamp(move_y, -1, 1);

                    if (child != null)
                    {
                        child.Update(this);
                    }
                }
            }

            public void RecordPosition(HashSet<Tuple<int, int>> set)
            {
                set.Add(new Tuple<int, int>(pos_x, pos_y));
            }

            public Knot child;
            public int pos_x = 0;
            public int pos_y = 0;
        }

        static void DisplayKnots(Knot[] knots, int min_x, int max_x, int min_y, int max_y, HashSet<Tuple<int, int>> positions)
        {
            int size_x = max_x - min_x + 1;
            int size_y = max_y - min_y + 1;

            char[,] grid = new char[size_x, size_y];

            for (int x = 0; x < size_x; x++)
            {
                for (int y = 0; y < size_y; y++)
                {
                    grid[x, y] = '.';
                }
            }

            foreach (Tuple<int, int> position in positions)
            {
                grid[position.Item1 - min_x, position.Item2 - min_y] = '#';
            }

            grid[0 - min_x, 0 - min_y] = 's';

            for (int k = knots.Length - 1; k >= 0; k--)
            {
                int pos_x = knots[k].pos_x - min_x;
                int pos_y = knots[k].pos_y - min_y;

                char c = k == 0 ? 'H' : (char)('0' + k);

                grid[pos_x, pos_y] = c;
            }

            Console.Clear();
            for (int y = 0; y < size_y; y++)
            {
                string line = string.Empty;
                for (int x = 0; x < size_x; x++)
                {
                    line += grid[x, y];
                }

                Console.WriteLine("{0}", line);
            }
            Console.ReadLine();
        }

        static void Puzzle09()
        {
            Knot[] knots = new Knot[10];
            for (int i = 0; i < 10; i++)
            {
                knots[i] = new Knot();
                if (i > 0)
                {
                    knots[i - 1].child = knots[i];
                }
            }

            Knot tail1 = knots[1];
            Knot tail2 = knots[9];

            HashSet<Tuple<int, int>> positions1 = new HashSet<Tuple<int, int>>();
            HashSet<Tuple<int, int>> positions2 = new HashSet<Tuple<int, int>>();

            string[] lines = System.IO.File.ReadAllLines("puzzles/input09.txt");

            tail1.RecordPosition(positions1);
            tail2.RecordPosition(positions2);

            int min_x = 0;
            int max_x = 0;
            int min_y = 0;
            int max_y = 0;

            foreach (string line in lines)
            {
                string[] instruction = line.Split(" ");
                int distance = int.Parse(instruction[1]);

                int move_x = 0;
                int move_y = 0;
                switch (instruction[0])
                {
                    case "U":
                        move_y = -1;
                        break;
                    case "D":
                        move_y = 1;
                        break;
                    case "L":
                        move_x = -1;
                        break;
                    case "R":
                        move_x = 1;
                        break;
                }

                for (int d = 0; d < distance; d++)
                {
                    knots[0].Move(move_x, move_y);
                    tail1.RecordPosition(positions1);
                    tail2.RecordPosition(positions2);

                    min_x = Math.Min(min_x, knots.Min(k => k.pos_x));
                    max_x = Math.Max(max_x, knots.Max(k => k.pos_x));
                    min_y = Math.Min(min_y, knots.Min(k => k.pos_y));
                    max_y = Math.Max(max_y, knots.Max(k => k.pos_y));
                }

                // DisplayKnots(knots, min_x, max_x, min_y, max_y, positions2);
            }

            Console.WriteLine("{0}", positions1.Count);
            Console.WriteLine("{0}", positions2.Count);
        }
    }
}
