using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC2022
{
    partial class Program
    {
        class Monkey21
        {
            public Monkey21(string line)
            {
                string[] left_and_right = line.Split(":");

                name = left_and_right[0];

                string[] words = left_and_right[1].Split(" ");

                if (words.Length == 4)
                {
                    monkey_names = new string[] { words[1], words[3] };
                    switch(words[2])
                    {
                        case "+":
                            operation = Operation.Add;
                            break;
                        case "-":
                            operation = Operation.Subtract;
                            break;
                        case "/":
                            operation = Operation.Divide;
                            break;
                        case "*":
                            operation = Operation.Multiply;
                            break;
                    }
                }
                else
                {
                    number = int.Parse(words[1]);
                }
            }

            public enum Operation
            {
                None,
                Add,
                Subtract,
                Multiply,
                Divide,
            }

            public bool Resolve(Dictionary<string, Monkey21> resolved_monkeys)
            {
                if (resolved_monkeys.TryGetValue(monkey_names[0], out Monkey21 other1) &&
                    resolved_monkeys.TryGetValue(monkey_names[1], out Monkey21 other2))
                {
                    Int64 value1 = other1.number.Value;
                    Int64 value2 = other2.number.Value;

                    switch(operation)
                    {
                        case Operation.Add:
                            number = value1 + value2;
                            break;
                        case Operation.Subtract:
                            number = value1 - value2;
                            break;
                        case Operation.Multiply:
                            number = value1 * value2;
                            break;
                        case Operation.Divide:
                            number = value1 / value2;
                            break;
                    }

                    return true;
                }

                return false;
            }

            public Int64? number;

            public readonly string name;
            public readonly string[] monkey_names = null;
            public readonly Operation operation = Operation.None;
        }

        static void ResolveMonkeys(Dictionary<string, Monkey21> resolved_monkeys, Dictionary<string, Monkey21> pending_monkeys)
        {
            while (pending_monkeys.Count > 0)
            {
                Monkey21[] finished = pending_monkeys.Where(x => x.Value.Resolve(resolved_monkeys)).Select(x => x.Value).ToArray();
                if (finished.Length == 0)
                {
                    break;
                }

                foreach (Monkey21 monkey in finished)
                {
                    pending_monkeys.Remove(monkey.name);
                    resolved_monkeys.Add(monkey.name, monkey);
                }
            }
        }
        static void Puzzle21Part1(string[] lines)
        {
            Monkey21[] monkeys1 = lines
                .Select(x => new Monkey21(x))
                .ToArray();

            Dictionary<string, Monkey21> resolved_monkeys1 = monkeys1.Where(x => x.number.HasValue).ToDictionary(x => x.name, x => x);
            Dictionary<string, Monkey21> pending_monkeys1 = monkeys1.Where(x => !x.number.HasValue).ToDictionary(x => x.name, x => x);

            ResolveMonkeys(resolved_monkeys1, pending_monkeys1);

            Console.WriteLine("{0}", resolved_monkeys1["root"].number.Value);
        }

        static void Puzzle21Part2(string[] lines)
        {
            Monkey21[] monkeys2 = lines
                .Select(x => new Monkey21(x))
                .ToArray();

            Dictionary<string, Monkey21> resolved_monkeys2 = monkeys2.Where(x => x.number.HasValue).ToDictionary(x => x.name, x => x);
            Dictionary<string, Monkey21> pending_monkeys2 = monkeys2.Where(x => !x.number.HasValue).ToDictionary(x => x.name, x => x);

            Monkey21 human = resolved_monkeys2["humn"];
            resolved_monkeys2.Remove(human.name);

            ResolveMonkeys(resolved_monkeys2, pending_monkeys2);

            Monkey21 root = pending_monkeys2["root"];
            resolved_monkeys2.TryGetValue(root.monkey_names[0], out Monkey21 temp1);
            resolved_monkeys2.TryGetValue(root.monkey_names[1], out Monkey21 temp2);

            Int64 target_value = temp1 != null ? temp1.number.Value : temp2.number.Value;

            string target_name = temp1 == null ? root.monkey_names[0] : root.monkey_names[1];

            while (target_name != human.name)
            {
                Monkey21 target = pending_monkeys2[target_name];
                resolved_monkeys2.TryGetValue(target.monkey_names[0], out Monkey21 monkey1);
                resolved_monkeys2.TryGetValue(target.monkey_names[1], out Monkey21 monkey2);

                Int64 value = monkey1 != null ? monkey1.number.Value : monkey2.number.Value;

                int invalid_index = monkey1 == null ? 0 : 1;

                target_name = target.monkey_names[invalid_index];

                switch (target.operation)
                {
                    case Monkey21.Operation.Add:
                        target_value -= value;
                        break;
                    case Monkey21.Operation.Subtract:
                        if (invalid_index == 0)
                        {
                            target_value += value;
                        }
                        else
                        {
                            target_value = value - target_value;
                        }
                        break;
                    case Monkey21.Operation.Multiply:
                        target_value /= value;
                        break;
                    case Monkey21.Operation.Divide:
                        if (invalid_index == 0)
                        {
                            target_value *= value;
                        }
                        else
                        {
                            target_value = value / target_value;
                        }
                        break;
                }
            }

            Console.WriteLine("{0}", target_value);
        }

        static void Puzzle21()
        {
            string[] lines = System.IO.File.ReadAllLines("puzzles/input21.txt");

            Puzzle21Part1(lines);
            Puzzle21Part2(lines);
        }
    }
}
