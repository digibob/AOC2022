using System;

namespace AOC2022
{
    partial class Program
    {
        class ElfCPU
        {
            enum InstructionType
            {
                Unknown,
                Noop,
                Add,
            }

            public delegate void CycleEventHandler();

            public void Interpret(string[] lines)
            {
                foreach (string line in lines)
                {
                    InstructionType instruction_type = InstructionType.Unknown;
                    int value = 0;
                    int cycle_count = 0;

                    if (line.StartsWith("noop"))
                    {
                        instruction_type = InstructionType.Noop;
                        cycle_count = 1;
                    }
                    else if (line.StartsWith("addx"))
                    {
                        instruction_type = InstructionType.Add;
                        value = int.Parse(line.Split(" ")[1]);
                        cycle_count = 2;
                    }

                    for (int instruction_cycle = 0; instruction_cycle < cycle_count; instruction_cycle++)
                    {
                        OnCycle();

                        Cycle++;
                    }

                    switch (instruction_type)
                    {
                        case InstructionType.Add:
                            RegisterX += value;
                            break;
                    }
                }
            }

            public event CycleEventHandler OnCycle;

            public int Cycle { get; private set; } = 1;

            public int RegisterX { get; private set; } = 1;
        }

        static void Puzzle10()
        {
            string[] lines = System.IO.File.ReadAllLines("puzzles/input10.txt");

            int strengths = 0;

            string[] out_lines = new string[6];

            ElfCPU cpu = new ElfCPU();

            cpu.OnCycle += () =>
            {
                int cycle = cpu.Cycle;

                if (((cycle - 20) % 40) == 0)
                {
                    strengths += cycle * cpu.RegisterX;
                }

                int out_line_index = (cycle - 1) / 40;
                int out_line_pos = (cycle - 1) % 40;

                if ((out_line_pos >= cpu.RegisterX - 1) &&
                    (out_line_pos <= cpu.RegisterX + 1))
                {
                    out_lines[out_line_index] += '#';
                }
                else
                {
                    out_lines[out_line_index] += '.';
                }
            };

            cpu.Interpret(lines);

            Console.WriteLine("{0}", strengths);

            foreach (string line in out_lines)
            {
                Console.WriteLine("{0}", line);
            }
        }
    }
}
