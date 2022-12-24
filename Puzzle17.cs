using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC2022
{
    partial class Program
    {
        static void Puzzle17()
        {
            Tuple<int, int>[] horizontal = new Tuple<int, int>[]
            {
                new Tuple<int, int>(0, 0),
                new Tuple<int, int>(1, 0),
                new Tuple<int, int>(2, 0),
                new Tuple<int, int>(3, 0),
            };

            Tuple<int, int>[] cross = new Tuple<int, int>[]
            {
                new Tuple<int, int>(1, 0),
                new Tuple<int, int>(0, 1),
                new Tuple<int, int>(1, 1),
                new Tuple<int, int>(2, 1),
                new Tuple<int, int>(1, 2),
            };

            Tuple<int, int>[] corner = new Tuple<int, int>[]
            {
                new Tuple<int, int>(0, 0),
                new Tuple<int, int>(1, 0),
                new Tuple<int, int>(2, 0),
                new Tuple<int, int>(2, 1),
                new Tuple<int, int>(2, 2),
            };

            Tuple<int, int>[] vertical = new Tuple<int, int>[]
            {
                new Tuple<int, int>(0, 0),
                new Tuple<int, int>(0, 1),
                new Tuple<int, int>(0, 2),
                new Tuple<int, int>(0, 3),
            };

            Tuple<int, int>[] block = new Tuple<int, int>[]
            {
                new Tuple<int, int>(0, 0),
                new Tuple<int, int>(0, 1),
                new Tuple<int, int>(1, 0),
                new Tuple<int, int>(1, 1),
            };

            Tuple<int, int>[][] shapes = new Tuple<int, int>[][]
            {
                horizontal,
                cross,
                corner,
                vertical,
                block,
            };

            int shape_index = 0;
            int input_index = 0;
            int cursor_y = 3;

            string input = System.IO.File.ReadAllLines("puzzles/input17.txt")[0];

            HashSet<Tuple<int, int>> grid = new HashSet<Tuple<int, int>>();

            bool IsValidPosition(int x, int y, Tuple<int, int>[] shape)
            {
                foreach (Tuple<int, int> offset in shape)
                {
                    Tuple<int, int> point = new Tuple<int, int>(x + offset.Item1, y + offset.Item2);
                    if (point.Item1 < 0 || point.Item1 > 6 || point.Item2 < 0)
                    {
                        return false;
                    }

                    if (grid.Contains(point))
                    {
                        return false;
                    }
                }

                return true;
            }

            void DropShape()
            {
                int cursor_x = 2;

                Tuple<int, int>[] shape = shapes[shape_index];

                while (true)
                {
                    int move_x = input[input_index % input.Length] == '<' ? -1 : 1;
                    input_index++;

                    int test_x = cursor_x + move_x;

                    if (IsValidPosition(test_x, cursor_y, shape))
                    {
                        cursor_x = test_x;
                    }

                    int test_y = cursor_y - 1;

                    if (IsValidPosition(cursor_x, test_y, shape))
                    {
                        cursor_y = test_y;
                        continue;
                    }

                    foreach (Tuple<int, int> offset in shape)
                    {
                        grid.Add(new Tuple<int, int>(cursor_x + offset.Item1, cursor_y + offset.Item2));
                    }

                    cursor_y = grid.Max(x => x.Item2) + 4;
                    break;
                }

                shape_index = (shape_index + 1) % shapes.Length;
            }

            void DropShapes(Int64 count)
            {
                for (Int64 i = 0; i < count; i++)
                {
                    DropShape();
                }
            }

            int GetHeight()
            {
                return grid.Max(x => x.Item2) + 1;
            }

            Int64 final_offset = 1000000000000;
            Int64 initial_offset = 2022;
            Int64 repeat_shape_count = 1730; // determined through observation of dropped shape offsets

            DropShapes(initial_offset);

            Int64 initial_height = GetHeight();

            DropShapes(repeat_shape_count);

            Int64 repeat_height = GetHeight();

            Int64 num_repeats_to_add = ((final_offset - initial_offset) / repeat_shape_count);

            Int64 final_shape_count = final_offset - ((num_repeats_to_add * repeat_shape_count) + initial_offset);

            DropShapes(final_shape_count);

            Int64 final_height = GetHeight();

            Int64 repeat_add_height = (repeat_height - initial_height) * (num_repeats_to_add - 1);

            Console.WriteLine("{0}", initial_height);
            Console.WriteLine("{0}", final_height + repeat_add_height);
        }
    }
}
