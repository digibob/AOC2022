using System;
using System.Linq;

namespace AOC2022
{
    partial class Program
    {
        static int SnafuToInt(char number)
        {
            return number switch
            {
                '2' => 2,
                '1' => 1,
                '-' => -1,
                '=' => -2,
                _ => 0,
            };
        }
        static char IntToSnafuChar(long number)
        {
            return number switch
            {
                2 => '2',
                1 => '1',
                -1 => '-',
                -2 => '=',
                _ => '0',
            };
        }

        static long SnafuToInt(string number)
        {
            long value = 0;
            long scale = 1;
            foreach (char c in number.Reverse())
            {
                value += scale * SnafuToInt(c);
                scale *= 5;
            }
            return value;
        }

        static string IntToSnafu(long number)
        {
            string output = string.Empty;

            long out_number = 2;
            long scale = 1;
            while (out_number < number)
            {
                scale *= 5;
                out_number += 2 * scale;
            }


            while (scale > 0)
            {
                long out_number1 = out_number - (scale * 4);
                long out_number2 = out_number - (scale * 3);
                long out_number3 = out_number - (scale * 2);
                long out_number4 = out_number - (scale * 1);
                long out_number5 = out_number - (scale * 0);

                if (out_number1 >= number)
                {
                    out_number = out_number1;
                    output += "=";
                }
                else if (out_number2 >= number)
                {
                    out_number = out_number2;
                    output += "-";
                }
                else if (out_number3 >= number)
                {
                    out_number = out_number3;
                    output += "0";
                }
                else if (out_number4 >= number)
                {
                    out_number = out_number4;
                    output += "1";
                }
                else if (out_number5 >= number)
                {
                    out_number = out_number5;
                    output += "2";
                }

                scale /= 5;
            }

            return output;
        }

        static void Puzzle25()
        {
            long sum = System.IO.File.ReadAllLines("puzzles/input25.txt")
                .Select(x => SnafuToInt(x))
                .Sum();

            Console.WriteLine(IntToSnafu(sum));
        }
    }
}
