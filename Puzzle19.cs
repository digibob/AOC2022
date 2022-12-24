using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC2022
{
    class Blueprint
    {
        public Blueprint(int multiplier, string line)
        {
            this.multiplier = multiplier;

            string[] descriptions = line.Split(":")[1].Split(".");

            string[] description_zero_words = descriptions[0].Split(" ");
            string[] description_one_words = descriptions[1].Split(" ");
            string[] description_two_words = descriptions[2].Split(" ");
            string[] description_three_words = descriptions[3].Split(" ");

            ore_robot_ore_cost = int.Parse(description_zero_words[5]);
            clay_robot_ore_cost = int.Parse(description_one_words[5]);
            obsidian_robot_ore_cost = int.Parse(description_two_words[5]);
            obsidian_robot_clay_cost = int.Parse(description_two_words[8]);
            geode_robot_ore_cost = int.Parse(description_three_words[5]);
            geode_robot_obsidian_cost = int.Parse(description_three_words[8]);

            max_ore_cost = Math.Max(Math.Max(ore_robot_ore_cost, clay_robot_ore_cost), Math.Max(obsidian_robot_ore_cost, geode_robot_ore_cost));
        }

        public readonly int multiplier = 0;
        public readonly int ore_robot_ore_cost = 0;
        public readonly int clay_robot_ore_cost = 0;
        public readonly int obsidian_robot_ore_cost = 0;
        public readonly int obsidian_robot_clay_cost = 0;
        public readonly int geode_robot_ore_cost = 0;
        public readonly int geode_robot_obsidian_cost = 0;

        public readonly int max_ore_cost;
    }

    class Puzzle19State
    {
        public int ore = 0;
        public int clay = 0;
        public int obsidian = 0;
        public int geodes = 0;

        public int ore_robots = 1;
        public int clay_robots = 0;
        public int obsidian_robots = 0;

        public int time = 1;

        public int skips = 0;

        public Puzzle19State Clone()
        {
            Puzzle19State new_state = new Puzzle19State
            {
                ore = ore,
                clay = clay,
                obsidian = obsidian,
                geodes = geodes,

                ore_robots = ore_robots,
                clay_robots = clay_robots,
                obsidian_robots = obsidian_robots,

                time = time,

                skips = skips
            };
            return new_state;
        }

        public string MemoizationString
        {
            get
            {
                return string.Format("{0}-{1}-{2}-{3}-{4}-{5}-{6}-{7}-{8}", ore, clay, obsidian, geodes, ore_robots, clay_robots, obsidian_robots, time, skips);
            }
        }
    }

    partial class Program
    {
        static void Puzzle19()
        {
            string[] lines = System.IO.File.ReadAllLines("puzzles/input19.txt");

            Blueprint[] all_blueprints = new Blueprint[lines.Length];
            for (int i = 0; i < lines.Length; i++)
            {
                all_blueprints[i] = new Blueprint(i + 1, lines[i]);
            }

            List<Tuple<Blueprint, int>> GetBestGeodes(int time_limit, IEnumerable<Blueprint> blueprints)
            {
                var scores = new List<Tuple<Blueprint, int>>();

                foreach (Blueprint blueprint in blueprints)
                {
                    Puzzle19State best_state = null;

                    Queue<Puzzle19State> state_queue = new Queue<Puzzle19State>();
                    state_queue.Enqueue(new Puzzle19State());

                    Dictionary<string, Puzzle19State> all_states = new Dictionary<string, Puzzle19State>();

                    void TryEnqueue(Puzzle19State state)
                    {
                        string memoization_string = state.MemoizationString;

                        if (all_states.ContainsKey(memoization_string))
                        {
                            return;
                        }

                        all_states.Add(memoization_string, state);
                        state_queue.Enqueue(state);
                    }

                    while (state_queue.TryDequeue(out Puzzle19State state))
                    {
                        bool skip_obsidian = (state.skips & (1 << 2)) != 0;
                        bool skip_clay = (state.skips & (1 << 1)) != 0;
                        bool skip_ore = (state.skips & (1 << 0)) != 0;

                        bool can_craft_geode_robot = state.ore >= blueprint.geode_robot_ore_cost && state.obsidian >= blueprint.geode_robot_obsidian_cost;
                        bool can_craft_obsidian_robot = !skip_obsidian && state.obsidian_robots < blueprint.geode_robot_obsidian_cost && state.ore >= blueprint.obsidian_robot_ore_cost && state.clay >= blueprint.obsidian_robot_clay_cost;
                        bool can_craft_clay_robot = !skip_clay && state.clay_robots < blueprint.obsidian_robot_clay_cost && state.ore >= blueprint.clay_robot_ore_cost;
                        bool can_craft_ore_robot = !skip_ore && state.ore_robots < blueprint.max_ore_cost && state.ore >= blueprint.ore_robot_ore_cost;

                        state.ore += state.ore_robots;
                        state.clay += state.clay_robots;
                        state.obsidian += state.obsidian_robots;

                        if (state.time == time_limit)
                        {
                            if (best_state == null || state.geodes > best_state.geodes)
                            {
                                best_state = state;
                            }
                            continue;
                        }

                        state.time++;

                        if (can_craft_geode_robot)
                        {
                            Puzzle19State new_state = state.Clone();
                            new_state.ore -= blueprint.geode_robot_ore_cost;
                            new_state.obsidian -= blueprint.geode_robot_obsidian_cost;
                            new_state.geodes += ((time_limit - state.time) + 1);
                            new_state.skips = 0;
                            TryEnqueue(new_state);
                        }
                        else
                        {
                            int skips = 0;

                            if (can_craft_obsidian_robot)
                            {
                                skips += 1 << 2;

                                Puzzle19State new_state = state.Clone();
                                new_state.ore -= blueprint.obsidian_robot_ore_cost;
                                new_state.clay -= blueprint.obsidian_robot_clay_cost;
                                new_state.obsidian_robots++;
                                new_state.skips = 0;
                                TryEnqueue(new_state);
                            }

                            if (can_craft_clay_robot)
                            {
                                skips += 1 << 1;

                                Puzzle19State new_state = state.Clone();
                                new_state.ore -= blueprint.clay_robot_ore_cost;
                                new_state.clay_robots++;
                                new_state.skips = 0;
                                TryEnqueue(new_state);
                            }

                            if (can_craft_ore_robot)
                            {
                                skips += 1 << 0;

                                Puzzle19State new_state = state.Clone();
                                new_state.ore -= blueprint.ore_robot_ore_cost;
                                new_state.ore_robots++;
                                new_state.skips = 0;
                                TryEnqueue(new_state);
                            }

                            state.skips |= skips;
                            TryEnqueue(state);
                        }
                    }

                    scores.Add(new Tuple<Blueprint, int>(blueprint, best_state.geodes));
                }

                return scores;
            }

            int score = GetBestGeodes(24, all_blueprints).Sum(x => x.Item2 * x.Item1.multiplier);
            int score2 = GetBestGeodes(32, all_blueprints.Take(3)).Aggregate(1, (x, y) => x * y.Item2);

            Console.WriteLine("{0}", score);
            Console.WriteLine("{0}", score2);
        }
    }
}
