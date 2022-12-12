using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC2022
{
    partial class Program
    {
        class MapNode
        {
            public MapNode(int x, int y, char height)
            {
                this.x = x;
                this.y = y;
                this.height = height;
            }

            public void Reset()
            {
                came_from = null;
                g_score = int.MaxValue;
                f_score = int.MaxValue;
            }

            public MapNode came_from;
            public int g_score = int.MaxValue;
            public int f_score = int.MaxValue;

            public int x;
            public int y;
            public char height;
            public List<MapNode> neighbours = new List<MapNode>();
        }

        static void Puzzle12()
        {
            string[] lines = System.IO.File.ReadAllLines("puzzles/input12.txt");

            int width = lines[0].Length;
            int height = lines.Length;

            MapNode[,] nodes = new MapNode[width, height];

            int start_x = 0;
            int start_y = 0;
            int end_x = 0;
            int end_y = 0;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    char c = lines[y][x];

                    if (c == 'S')
                    {
                        start_x = x;
                        start_y = y;

                        c = 'a';
                    }
                    else if (c == 'E')
                    {
                        end_x = x;
                        end_y = y;

                        c = 'z';
                    }

                    nodes[x, y] = new MapNode(x, y, c);
                }
            }

            foreach (MapNode node in nodes)
            {
                if (node.x > 0)
                {
                    MapNode other = nodes[node.x - 1, node.y];
                    if (other.height <= node.height + 1)
                    {
                        node.neighbours.Add(other);
                    }
                }

                if (node.x < width - 1)
                {
                    MapNode other = nodes[node.x + 1, node.y];
                    if (other.height <= node.height + 1)
                    {
                        node.neighbours.Add(other);
                    }
                }

                if (node.y > 0)
                {
                    MapNode other = nodes[node.x, node.y - 1];
                    if (other.height <= node.height + 1)
                    {
                        node.neighbours.Add(other);
                    }
                }

                if (node.y < height - 1)
                {
                    MapNode other = nodes[node.x, node.y + 1];
                    if (other.height <= node.height + 1)
                    {
                        node.neighbours.Add(other);
                    }
                }
            };

            MapNode goal = nodes[end_x, end_y];

            Console.WriteLine("{0}", MakeBestPath(new List<MapNode>{ nodes[start_x, start_y] }, goal));

            List<MapNode> all_starts = new List<MapNode>();
            foreach (MapNode node in nodes)
            {
                node.Reset();

                if (node.height == 'a')
                {
                    all_starts.Add(node);
                }
            }

            Console.WriteLine("{0}", MakeBestPath(all_starts, goal));
        }

        static int MakeBestPath(List<MapNode> starts, MapNode goal)
        {
            int h(MapNode node)
            {
                return Math.Abs(node.x - goal.x) + Math.Abs(node.y - goal.y);
            }

            int d(MapNode current, MapNode other)
            {
                return (current.height - other.height) + 1;
            }

            foreach (MapNode start in starts)
            {
                start.g_score = 0;
                start.f_score = h(start);
            }

            HashSet<MapNode> open_set = new HashSet<MapNode>(starts);

            MapNode best = null;

            while (open_set.Count > 0)
            {
                MapNode current = open_set.Aggregate((best, x) =>
                    (best == null || (x.f_score < best.f_score)) ? x : best);

                if (current == goal)
                {
                    best = current;
                    break;
                }

                open_set.Remove(current);

                foreach (MapNode neighbour in current.neighbours)
                {
                    int new_g_score = current.g_score + d(current, neighbour);
                    if (new_g_score < neighbour.g_score)
                    {
                        neighbour.came_from = current;
                        neighbour.g_score = new_g_score;
                        neighbour.f_score = new_g_score + h(neighbour);
                        open_set.Add(neighbour);
                    }
                }
            }

            if (best == null)
            {
                return int.MaxValue;
            }

            int count = 0;

            MapNode check_node = best.came_from;
            while (check_node != null)
            {
                check_node = check_node.came_from;
                count++;
            }

            return count;
        }
    }
}
