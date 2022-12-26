using System;
using System.Collections.Generic;

namespace AOC2022
{
    class Blizzard24
    {
        public Blizzard24(int x, int y, char type)
        {
            this.x = x;
            this.y = y;
            this.type = type;
        }

        public int x;
        public int y;
        public char type;
    }
    class State24
    {
        public int time = 0;
        public int time_modulo = 0;
        public int elf_x = 0;
        public int elf_y = 0;

        public string MemoizationString
        {
            get
            {
                return string.Format("{0}-{1}-{2}", elf_x, elf_y, time_modulo);
            }
        }
    }

    partial class Program
    {
        static void Puzzle24()
        {
            string[] lines = System.IO.File.ReadAllLines("puzzles/input24.txt");

            List<Blizzard24> blizzards = new List<Blizzard24>();

            for (int y = 1; y < lines.Length - 1; y++)
            {
                string line = lines[y];

                for (int x = 1; x < line.Length - 1; x++)
                {
                    char type = line[x];
                    if (type == '.')
                    {
                        continue;
                    }
                    blizzards.Add(new Blizzard24(x - 1, y - 1, type));
                }
            }

            int max_x = lines[0].Length - 2;
            int max_y = lines.Length - 2;

            Tuple<int, int> GetBlizzardPosition(Blizzard24 blizzard, int time)
            {
                static int Mod(int k, int n)
                {
                    k %= n;
                    return k < 0 ? k + n : k;
                }

                return blizzard.type switch
                {
                    '>' => new Tuple<int, int>(Mod(blizzard.x + time, max_x), blizzard.y),
                    '^' => new Tuple<int, int>(blizzard.x, Mod(blizzard.y - time, max_y)),
                    'v' => new Tuple<int, int>(blizzard.x, Mod(blizzard.y + time, max_y)),
                    _ => new Tuple<int, int>(Mod(blizzard.x - time, max_x), blizzard.y),
                };
            }

            HashSet<Tuple<int, int>> GenerateBlizzards(int time)
            {
                HashSet<Tuple<int, int>> occupied_points = new HashSet<Tuple<int, int>>();

                foreach (Blizzard24 blizzard in blizzards)
                {
                    occupied_points.Add(GetBlizzardPosition(blizzard, time));
                }

                return occupied_points;
            }

            int LCM = 600;

            HashSet<Tuple<int, int>>[] occupied_points_array = new HashSet<Tuple<int, int>>[LCM];
            for (int time_index = 0; time_index < LCM; time_index++)
            {
                occupied_points_array[time_index] = GenerateBlizzards(time_index);
            }

            int RunSimulation(int start_x, int start_y, int target_x, int target_y, int start_time)
            {
                Dictionary<string, State24> all_states = new Dictionary<string, State24>();

                Queue<State24> state_queue = new Queue<State24>();

                void TryEnqueueState(State24 new_state)
                {
                    string memoization_string = new_state.MemoizationString;
                    if (all_states.ContainsKey(memoization_string))
                    {
                        return;
                    }
                    all_states.Add(memoization_string, new_state);
                    state_queue.Enqueue(new_state);
                }

                for (int time_index = 0; time_index < LCM; time_index++)
                {
                    int offset_start_time = start_time + time_index;

                    HashSet<Tuple<int, int>> occupied_points = occupied_points_array[offset_start_time % LCM];
                    if (!occupied_points.Contains(new Tuple<int, int>(start_x, start_y)))
                    {
                        TryEnqueueState(new State24
                        {
                            elf_x = start_x,
                            elf_y = start_y,
                            time = offset_start_time,
                            time_modulo = offset_start_time % LCM
                        });
                    }
                }

                int best_time = int.MaxValue;

                while (state_queue.TryDequeue(out State24 state))
                {
                    if ((state.elf_x == target_x)
                        && (state.elf_y == target_y))
                    {
                        best_time = Math.Min(best_time, state.time + 1);
                        continue;
                    }

                    int next_time = state.time + 1;

                    HashSet<Tuple<int, int>> occupied_points = occupied_points_array[next_time % LCM];

                    void CheckPoint(Tuple<int, int> point)
                    {
                        if (!occupied_points.Contains(point))
                        {
                            TryEnqueueState(new State24
                            {
                                time = next_time,
                                time_modulo = next_time % LCM,
                                elf_x = point.Item1,
                                elf_y = point.Item2
                            });
                        }
                    }

                    if (state.elf_x + 1 < max_x)
                    {
                        CheckPoint(new Tuple<int, int>(state.elf_x + 1, state.elf_y));
                    }

                    if (state.elf_y + 1 < max_y)
                    {
                        CheckPoint(new Tuple<int, int>(state.elf_x, state.elf_y + 1));
                    }

                    CheckPoint(new Tuple<int, int>(state.elf_x, state.elf_y));

                    if (state.elf_x - 1 >= 0)
                    {
                        CheckPoint(new Tuple<int, int>(state.elf_x - 1, state.elf_y));
                    }

                    if (state.elf_y - 1 >= 0)
                    {
                        CheckPoint(new Tuple<int, int>(state.elf_x, state.elf_y - 1));
                    }
                }

                return best_time;
            }

            int best_time1 = RunSimulation(0, 0, max_x - 1, max_y - 1, 0);
            int best_time2 = RunSimulation(max_x - 1, max_y - 1, 0, 0, best_time1 + 1);
            int best_time3 = RunSimulation(0, 0, max_x - 1, max_y - 1, best_time2 + 1);

            Console.WriteLine("{0}", best_time1);
            Console.WriteLine("{0}", best_time3);
        }
    }
}
