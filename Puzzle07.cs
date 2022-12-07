using System.Collections.Generic;

namespace AOC2022
{
    partial class Program
    {
        static void Puzzle07()
        {
            string[] lines = System.IO.File.ReadAllLines("puzzles/input07.txt");

            Directory root = new Directory();
            root.name = "/";

            List<Directory> all_dirs = new List<Directory>();
            all_dirs.Add(root);

            Directory current_dir = root;

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];

                if (line.StartsWith("$ cd "))
                {
                    string dir_name = line.Substring(5);
                    if (dir_name == "..")
                    {
                        current_dir = current_dir.parent;
                    }
                    else if (dir_name == "/")
                    {
                        current_dir = root;
                    }
                    else
                    {
                        current_dir = current_dir.AddDir(dir_name);
                        all_dirs.Add(current_dir);
                    }
                }
                else if (line.StartsWith("$ ls"))
                {
                    while (i + 1 < lines.Length
                        && !lines[i + 1].StartsWith("$"))
                    {
                        i++;
                        line = lines[i];

                        if (!line.StartsWith("dir"))
                        {
                            string[] size_and_name = line.Split(" ");
                            current_dir.AddFile(size_and_name[1], int.Parse(size_and_name[0]));
                        }
                    }
                }
            }

            int space_to_remove = root.size - 40000000;

            Directory dir_to_remove = null;
            int small_total_size = 0;
            foreach (Directory dir in all_dirs)
            {
                if (dir.size <= 100000)
                {
                    small_total_size += dir.size;
                }

                if (dir.size >= space_to_remove)
                {
                    if (dir_to_remove == null || dir.size < dir_to_remove.size)
                    {
                        dir_to_remove = dir;
                    }
                }
            }

            System.Console.WriteLine("{0}", small_total_size);
            System.Console.WriteLine("{0}", dir_to_remove.size);
        }
    }
}
