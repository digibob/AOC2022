using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Numerics;

namespace AOC2022
{
    partial class Program
    {
        class Monkey
        {
            public Monkey(string[] lines, int start_index)
            {
                items_held = lines[start_index + 1].Substring(18).Split(", ").Select(x => UInt64.Parse(x)).ToList();
                string[] operation_values = lines[start_index + 2].Substring(23).Split(" ");
                operation = operation_values[0] == "*" ? Monkey.Operation.Multiply : Monkey.Operation.Plus;
                operation_value = operation_values[1] == "old" ? -1 : int.Parse(operation_values[1]);
                divisor = UInt64.Parse(lines[start_index + 3].Substring(21));
                true_monkey_index = int.Parse(lines[start_index + 4].Substring(29));
                false_monkey_index = int.Parse(lines[start_index + 5].Substring(30));
            }

            public enum Operation
            {
                Plus,
                Multiply,
            }

            public readonly List<UInt64> items_held;
            public readonly Operation operation;
            public readonly int operation_value = -1;
            public readonly UInt64 divisor = 0;
            public readonly int false_monkey_index = 0;
            public readonly int true_monkey_index = 0;

            public UInt64 items_inspected = 0;
        }

        static void Puzzle11()
        {
            string[] lines = System.IO.File.ReadAllLines("puzzles/input11.txt");

            UInt64 product_of_divisors = 1;

            List<Monkey> monkeys1 = new List<Monkey>();
            List<Monkey> monkeys2 = new List<Monkey>();
            for (int i = 0; i < lines.Length; i += 7)
            {
                monkeys1.Add(new Monkey(lines, i));
                monkeys2.Add(new Monkey(lines, i));

                product_of_divisors *= monkeys2.Last().divisor;
            }

            DoMonkeyBusiness(x => x / 3, monkeys1, 20);
            DoMonkeyBusiness(x => x % product_of_divisors, monkeys2, 10000);
        }

        private static void DoMonkeyBusiness(Func<UInt64, UInt64> apply_relief, List<Monkey> monkeys, int num_rounds)
        {
            for (int round_index = 0; round_index < num_rounds; round_index++)
            {
                foreach (Monkey monkey in monkeys)
                {
                    foreach (UInt64 item_value in monkey.items_held)
                    {
                        UInt64 operation_value = monkey.operation_value == -1 ? item_value : (UInt64)monkey.operation_value;
                        UInt64 new_item_value = 0;

                        switch (monkey.operation)
                        {
                            case Monkey.Operation.Multiply:
                                new_item_value = item_value * operation_value;
                                break;
                            case Monkey.Operation.Plus:
                                new_item_value = item_value + operation_value;
                                break;
                        }

                        new_item_value = apply_relief(new_item_value);

                        bool is_divisible = (new_item_value % monkey.divisor) == 0;
                        int new_monkey_index = is_divisible ? monkey.true_monkey_index : monkey.false_monkey_index;
                        monkeys[new_monkey_index].items_held.Add(new_item_value);
                        monkey.items_inspected++;
                    }

                    monkey.items_held.Clear();
                }
            }

            UInt64[] values = monkeys.OrderByDescending(x => x.items_inspected).Take(2).Select(x => x.items_inspected).ToArray();
            Console.WriteLine("{0}", values[0] * values[1]);
        }
    }
}
