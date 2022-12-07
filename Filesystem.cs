using System.Collections.Generic;

namespace AOC2022
{
    class Directory
    {
        public int size = 0;
        public string name = string.Empty;
        public Directory parent = null;
        public List<Directory> dirs = new List<Directory>();
        public List<File> files = new List<File>();

        public void AddFile(string file_name, int file_size)
        {
            File file = new File();
            file.size = file_size;
            file.name = file_name;

            files.Add(file);

            Directory dir = this;
            while (dir != null)
            {
                dir.size += file_size;
                dir = dir.parent;
            }
        }

        public Directory AddDir(string dir_name)
        {
            Directory new_dir = new Directory();
            new_dir.parent = this;
            new_dir.name = this.name + dir_name + "/";
            dirs.Add(new_dir);
            return new_dir;
        }
    }
    class File
    {
        public string name;
        public int size;
    }
}
