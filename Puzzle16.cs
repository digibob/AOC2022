using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC2022
{
    partial class Program
    {
        class Puzzle16SolveState
        {
            public Puzzle16State best_state;
        }

        class Puzzle16Node
        {
            public Puzzle16Node(string line)
            {
                string[] valves_and_tunnels = line.Split(";");

                string[] name_and_rate = valves_and_tunnels[0].Split(" has flow rate=");

                name = name_and_rate[0][6..];
                rate = int.Parse(name_and_rate[1]);

                targets = valves_and_tunnels[1].Contains("valves")
                    ? valves_and_tunnels[1][24..].Split(", ")
                    : new string[] { valves_and_tunnels[1][23..] };
            }

            public void BuildTravelTimes()
            {
                int distance = 1;

                HashSet<Puzzle16Node> nodes_to_visit = new HashSet<Puzzle16Node>(target_nodes);

                while (nodes_to_visit.Count > 0)
                {
                    Puzzle16Node[] current_set = nodes_to_visit.ToArray();
                    nodes_to_visit.Clear();

                    foreach (Puzzle16Node other in current_set)
                    {
                        if (!travel_times.ContainsKey(other) && other != this)
                        {
                            travel_times[other] = distance;

                            foreach (Puzzle16Node test_node in other.target_nodes)
                            {
                                nodes_to_visit.Add(test_node);
                            }
                        }
                    }

                    distance++;
                }
            }

            public string name;
            public int rate;
            public string[] targets;
            public Puzzle16Node[] target_nodes;
            public Dictionary<Puzzle16Node, int> travel_times = new Dictionary<Puzzle16Node, int>();
        }

        class Puzzle16State
        {
            public Puzzle16State Clone()
            {
                Puzzle16State clone = new Puzzle16State(arrival_time[0], arrival_time[1]);
                clone.current_node[0] = current_node[0];
                clone.current_node[1] = current_node[1];
                clone.pressure_released = pressure_released;
                clone.rates = rates.ToDictionary(x => x.Key, x => x.Value);
                return clone;
            }

            private Puzzle16State(int start_time_self, int start_time_elephant)
            {
                arrival_time = new int[] { start_time_self, start_time_elephant };
            }

            public Puzzle16State(Puzzle16Node[] nodes, Puzzle16Node current_node, int start_time_self, int start_time_elephant)
            {
                this.current_node[0] = current_node;
                this.current_node[1] = current_node;
                rates = nodes
                    .Where(x => x.rate > 0)
                    .ToDictionary(x => x, x => x.rate);
                arrival_time = new int[] { start_time_self, start_time_elephant };
            }

            public void TakePath(Puzzle16Node node, int index)
            {
                int cost = current_node[index].travel_times[node] + 1;
                arrival_time[index] -= cost;
                current_node[index] = node;
                if (arrival_time[index] > 0)
                {
                    pressure_released += arrival_time[index] * rates[node];
                }
                rates.Remove(node);
            }

            public int pressure_released = 0;
            public int[] arrival_time;
            public Puzzle16Node[] current_node = new Puzzle16Node[2];
            public Dictionary<Puzzle16Node, int> rates;
        }

        static int PickBest(Puzzle16State current)
        {
            Puzzle16SolveState state = new Puzzle16SolveState();
            PickNext(current, state);
            return state.best_state.pressure_released;
        }

        static void PickNext(Puzzle16State current, Puzzle16SolveState state)
        {
            int next_index = current.arrival_time[0] > current.arrival_time[1] ? 0 : 1;

            foreach (var rate in current.rates)
            {
                TakePath(current, rate.Key, next_index, state);
            }
        }

        static void TakePath(Puzzle16State current, Puzzle16Node node, int index, Puzzle16SolveState state)
        {
            Puzzle16State new_state = current.Clone();
            new_state.TakePath(node, index);
            if ((new_state.arrival_time[0] < 0 && new_state.arrival_time[1] < 0)
                || new_state.rates.Count == 0)
            {
                if (state.best_state == null || new_state.pressure_released > state.best_state.pressure_released)
                {
                    state.best_state = new_state;
                }

                return;
            }

            if (state.best_state != null)
            {
                int time_remaining = Math.Max(new_state.arrival_time[0], new_state.arrival_time[1]);

                int best_sum = new_state.rates.Sum(x => x.Value) * time_remaining;
                if ((new_state.pressure_released + best_sum) <= state.best_state.pressure_released)
                {
                    return;
                }
            }

            PickNext(new_state, state);
        }

        static void Puzzle16()
        {
            string[] lines = System.IO.File.ReadAllLines("puzzles/input16.txt");

            Puzzle16Node[] nodes = lines.Select(x => new Puzzle16Node(x)).ToArray();
            Dictionary<string, Puzzle16Node> node_map = nodes.ToDictionary(x => x.name, x => x);

            foreach (Puzzle16Node node in nodes)
            {
                node.target_nodes = node.targets.Select(x => node_map[x]).ToArray();
            }

            foreach (Puzzle16Node node in nodes)
            {
                node.BuildTravelTimes();
            }

            Puzzle16Node start_node = node_map["AA"];

            Console.WriteLine("{0}", PickBest(new Puzzle16State(nodes, start_node, 30, -1)));
            Console.WriteLine("{0}", PickBest(new Puzzle16State(nodes, start_node, 26, 26)));
        }
    }
}
