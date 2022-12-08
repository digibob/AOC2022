using System;

namespace AOC2022
{
    partial class Program
    {
        static void Puzzle08()
        {
            string[] lines = System.IO.File.ReadAllLines("puzzles/input08.txt");

            int width = lines[0].Length;
            int height = lines.Length;

            int[,] grid = new int[width, height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    grid[x, y] = lines[y][x] - '0';
                }
            }

            Func<int, int, bool> ScanTree = (int x_in, int y_in) =>
            {
                int value = grid[x_in, y_in];

                Func<int, int, bool> XRangeCheck = (int start, int end) =>
                {
                    for (int x = start; x < end; x++)
                    {
                        if (grid[x, y_in] >= value)
                        {
                            return false;
                        }
                    }

                    return true;
                };

                Func<int, int, bool> YRangeCheck = (int start, int end) =>
                {
                    for (int y = start; y < end; y++)
                    {
                        if (grid[x_in, y] >= value)
                        {
                            return false;
                        }
                    }

                    return true;
                };

                return XRangeCheck(0, x_in) ||
                    XRangeCheck(x_in + 1, width) ||
                    YRangeCheck(0, y_in) ||
                    YRangeCheck(y_in + 1, height);
            };

            Func<int, int, int> ScoreTree = (int x_in, int y_in) =>
            {
                int value = grid[x_in, y_in];

                int count1 = 0;
                int count2 = 0;
                int count3 = 0;
                int count4 = 0;

                for (int x = x_in - 1; x >= 0; x--)
                {
                    count1++;

                    if (grid[x, y_in] >= value)
                    {
                        break;
                    }
                }

                for (int x = x_in + 1; x < width; x++)
                {
                    count2++;

                    if (grid[x, y_in] >= value)
                    {
                        break;
                    }
                }

                for (int y = y_in - 1; y >= 0; y--)
                {
                    count3++;

                    if (grid[x_in, y] >= value)
                    {
                        break;
                    }
                }

                for (int y = y_in + 1; y < height; y++)
                {
                    count4++;

                    if (grid[x_in, y] >= value)
                    {
                        break;
                    }
                }

                return count1 * count2 * count3 * count4;
            };
            
            int count = 0;
            int score = 0;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (ScanTree(x, y))
                    {
                        count++;
                    }

                    score = Math.Max(score, ScoreTree(x, y));
                }
            }

            Console.WriteLine("{0}", count);
            Console.WriteLine("{0}", score);
        }
    }
}
