using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC2022
{
    using Point22 = Tuple<int, int>;
    using Offset22 = Tuple<int, int>;

    partial class Program
    {
        class Puzzle22Node
        {
            public Puzzle22Node(Point22 position)
            {
                this.position = position;
            }

            public readonly Point22 position;
            public readonly Puzzle22Node[] links = new Puzzle22Node[4];
            public readonly int[] link_directions = new int[4];
        }

        private static readonly Puzzle22Node wall = new Puzzle22Node(new Point22(-1, -1));

        private static int WalkPath(string directions, Puzzle22Node node)
        {
            int facing = 0;

            int index = 0;
            while (index < directions.Length)
            {
                int start_index = index;
                int end_index = start_index;

                while (end_index < directions.Length && char.IsDigit(directions[end_index]))
                {
                    end_index++;
                }

                int move = int.Parse(directions[start_index..end_index]);

                for (int m = 0; m < move; m++)
                {
                    var next_node = node.links[facing];

                    if (next_node != wall)
                    {
                        facing = node.link_directions[facing];
                        node = next_node;
                    }
                    else
                    {
                        break;
                    }
                }

                if (end_index < directions.Length)
                {
                    char turn = directions[end_index];

                    int offset = turn == 'L' ? 3 : 1;

                    facing = (facing + offset) % 4;
                }

                index = end_index + 1;
            }

            return ((node.position.Item2 + 1) * 1000) + ((node.position.Item1 + 1) * 4) + facing;
        }

        static void Puzzle22()
        {
            string[] lines = System.IO.File.ReadAllLines("puzzles/input22.txt");

            Dictionary<Point22, char> grid = new Dictionary<Point22, char>();

            for (int y = 0; y < lines.Length - 2; y++)
            {
                string line = lines[y];

                for (int x = 0; x < line.Length; x++)
                {
                    char c = line[x];
                    if (c == ' ')
                    {
                        continue;
                    }

                    grid.Add(new Point22(x, y), c);
                }
            }

            Dictionary<Point22, Puzzle22Node> nodes1
                = grid.Where(x => x.Value == '.')
                .ToDictionary(x => x.Key, x => new Puzzle22Node(x.Key));

            Dictionary<Point22, Puzzle22Node> nodes2
                = grid.Where(x => x.Value == '.')
                .ToDictionary(x => x.Key, x => new Puzzle22Node(x.Key));

            void MakeNeighbourLinks(Dictionary<Point22, Puzzle22Node> nodes)
            {
                foreach (var KV in nodes)
                {
                    Point22 point = KV.Key;

                    Point22 right = new Point22(point.Item1 + 1, point.Item2);
                    if (grid.TryGetValue(right, out char right_c))
                    {
                        KV.Value.links[0] = right_c == '.' ? nodes[right] : wall;
                        KV.Value.link_directions[0] = 0;
                    }

                    Point22 down = new Point22(point.Item1, point.Item2 + 1);
                    if (grid.TryGetValue(down, out char down_c))
                    {
                        KV.Value.links[1] = down_c == '.' ? nodes[down] : wall;
                        KV.Value.link_directions[1] = 1;
                    }

                    Point22 left = new Point22(point.Item1 - 1, point.Item2);
                    if (grid.TryGetValue(left, out char left_c))
                    {
                        KV.Value.links[2] = left_c == '.' ? nodes[left] : wall;
                        KV.Value.link_directions[2] = 2;
                    }

                    Point22 up = new Point22(point.Item1, point.Item2 - 1);
                    if (grid.TryGetValue(up, out char up_c))
                    {
                        KV.Value.links[3] = up_c == '.' ? nodes[up] : wall;
                        KV.Value.link_directions[3] = 3;
                    }
                }
            }

            void MakeWrappedLinksPart1(Dictionary<Point22, Puzzle22Node> nodes)
            {
                foreach (var KV in nodes)
                {
                    Point22 point = KV.Key;

                    if (KV.Value.links[0] == null)
                    {
                        int min_x = grid.Where(x => x.Key.Item2 == point.Item2).Min(x => x.Key.Item1);

                        var wrap_point = new Point22(min_x, point.Item2);

                        KV.Value.links[0] = grid[wrap_point] == '.' ? nodes[wrap_point] : wall;
                        KV.Value.link_directions[0] = 0;
                    }

                    if (KV.Value.links[1] == null)
                    {
                        int min_y = grid.Where(x => x.Key.Item1 == point.Item1).Min(x => x.Key.Item2);

                        var wrap_point = new Point22(point.Item1, min_y);

                        KV.Value.links[1] = grid[wrap_point] == '.' ? nodes[wrap_point] : wall;
                        KV.Value.link_directions[1] = 1;
                    }

                    if (KV.Value.links[2] == null)
                    {
                        int max_x = grid.Where(x => x.Key.Item2 == point.Item2).Max(x => x.Key.Item1);

                        var wrap_point = new Point22(max_x, point.Item2);

                        KV.Value.links[2] = grid[wrap_point] == '.' ? nodes[wrap_point] : wall;
                        KV.Value.link_directions[2] = 2;
                    }

                    if (KV.Value.links[3] == null)
                    {
                        int max_y = grid.Where(x => x.Key.Item1 == point.Item1).Max(x => x.Key.Item2);

                        var wrap_point = new Point22(point.Item1, max_y);

                        KV.Value.links[3] = grid[wrap_point] == '.' ? nodes[wrap_point] : wall;
                        KV.Value.link_directions[3] = 3;
                    }
                }
            }

            void MakeWrappedLinksPart2(Dictionary<Point22, Puzzle22Node> nodes)
            {
                void StichSides(int width, int facing1, Point22 start1, Offset22 offset1, int facing2, Point22 start2, Offset22 offset2)
                {
                    for (int i = 0; i < width; i++)
                    {
                        Point22 point1 = new Point22(start1.Item1 + (offset1.Item1 * i), start1.Item2 + (offset1.Item2 * i));
                        Point22 point2 = new Point22(start2.Item1 + (offset2.Item1 * i), start2.Item2 + (offset2.Item2 * i));

                        if (grid[point1] == '.')
                        {
                            Puzzle22Node node1 = nodes[point1];
                            if (node1.links[facing1] != null)
                            {
                                throw new Exception();
                            }

                            node1.links[facing1] = grid[point2] == '.' ? nodes[point2] : wall;
                            node1.link_directions[facing1] = (facing2 + 2) % 4;
                        }

                        if (grid[point2] == '.')
                        {
                            Puzzle22Node node2 = nodes[point2];

                            if (node2.links[facing2] != null)
                            {
                                throw new Exception();
                            }

                            node2.links[facing2] = grid[point1] == '.' ? nodes[point1] : wall;
                            node2.link_directions[facing2] = (facing1 + 2) % 4;
                        }
                    }
                }

                // Test
                //StichSides(4, 2, new Point22(8, 0), new Offset22(0, 1), 3, new Point22(4, 4), new Offset22(1, 0)); // 1L-3U
                //StichSides(4, 3, new Point22(8, 0), new Offset22(1, 0), 3, new Point22(3, 4), new Offset22(-1, 0)); // 1U-2U
                //StichSides(4, 1, new Point22(4, 7), new Offset22(1, 0), 2, new Point22(8, 11), new Offset22(0, -1)); // 3D-5L
                //StichSides(4, 1, new Point22(0, 7), new Offset22(1, 0), 1, new Point22(11, 11), new Offset22(-1, 0)); // 2D-5D                
                //StichSides(4, 2, new Point22(0, 4), new Offset22(0, 1), 1, new Point22(15, 11), new Offset22(-1, 0)); // 2L-6D                
                //StichSides(4, 0, new Point22(11, 0), new Offset22(0, 1), 0, new Point22(15, 11), new Offset22(0, -1)); // 1R-6R
                //StichSides(4, 0, new Point22(11, 4), new Offset22(0, 1), 3, new Point22(15, 8), new Offset22(-1, 0)); // 4R-6U

                // Real
                StichSides(50, 0, new Point22(149, 49), new Offset22(0, -1), 0, new Point22(99, 100), new Offset22(0, 1)); // 2R-5R
                StichSides(50, 1, new Point22(100, 49), new Offset22(1, 0), 0, new Point22(99, 50), new Offset22(0, 1)); // 2D-3R
                StichSides(50, 1, new Point22(50, 149), new Offset22(1, 0), 0, new Point22(49, 150), new Offset22(0, 1)); // 5R-6R
                StichSides(50, 2, new Point22(50, 0), new Offset22(0, 1), 2, new Point22(0, 149), new Offset22(0, -1)); // 1L-4L
                StichSides(50, 2, new Point22(50, 50), new Offset22(0, 1), 3, new Point22(0, 100), new Offset22(1, 0)); // 3L-4U
                StichSides(50, 3, new Point22(50, 0), new Offset22(1, 0), 2, new Point22(0, 150), new Offset22(0, 1)); // 1U-6L
                StichSides(50, 3, new Point22(100, 0), new Offset22(1, 0), 1, new Point22(0, 199), new Offset22(1, 0)); // 2U-6D
            }

            MakeNeighbourLinks(nodes1);
            MakeWrappedLinksPart1(nodes1);

            MakeNeighbourLinks(nodes2);
            MakeWrappedLinksPart2(nodes2);

            Console.WriteLine("{0}", WalkPath(lines.Last(), nodes1.First().Value));
            Console.WriteLine("{0}", WalkPath(lines.Last(), nodes2.First().Value));
        }
    }
}
