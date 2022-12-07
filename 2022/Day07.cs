using AdventOfCode.Library;

    namespace AdventOfCode._2022;

    public class Day7 : DayEngine
    {
        public override string[] TestInput => new string[]
        {
            "$ cd /",
            "$ ls",
            "dir a",
            "14848514 b.txt",
            "8504156 c.dat",
            "dir d",
            "$ cd a",
            "$ ls",
            "dir e",
            "29116 f",
            "2557 g",
            "62596 h.lst",
            "$ cd e",
            "$ ls",
            "584 i",
            "$ cd ..",
            "$ cd ..",
            "$ cd d",
            "$ ls",
            "4060174 j",
            "8033020 d.log",
            "5626152 d.ext",
            "7214296 k",
        };

        protected override object HandlePart1(string[] input)
        {
            var (root, allDirs) = BuildFileSystem(input);
            
            return allDirs.Select(x => x.GetSize()).Where(x => x <= 100_000).Sum();
        }

        protected override object HandlePart2(string[] input)
        {
            var (root, allDirs) = BuildFileSystem(input);

            var totalDiskSpaceAvailable = 70_000_000;
            var requiredSpaceFree = 30_000_000;
            
            var currentSpaceFree = totalDiskSpaceAvailable - root.GetSize();
            var requiredDeletedSpace = requiredSpaceFree - currentSpaceFree;

            var dirSizeToDelete = allDirs
                .Select(x => x.GetSize())
                .OrderBy(x => x)
                .SkipWhile(x => x < requiredDeletedSpace)
                .First();


            return dirSizeToDelete;
        }

        
        

        private (Directory Root, List<Directory> ALlDirs) BuildFileSystem(string[] input)
        {
            var allDirs = new List<Directory>();
            
            Directory currentDir = new Directory("/", null, new List<FileSystemEntry>());
            allDirs.Add(currentDir);
            var currentCmd = "";
            var fileSystemRoot = currentDir;
            
            foreach (var cmd in input)
            {
                if (cmd.StartsWith("$"))
                {
                    currentCmd = cmd[2..];
                    if (currentCmd == "cd /")
                    {
                        currentDir = fileSystemRoot;
                    }
                    else if (currentCmd.StartsWith("cd"))
                    {
                        var targetDir = currentCmd.Split(' ').Last();

                        if (targetDir == "..")
                        {
                            currentDir = currentDir.Parent!;
                        }
                        else
                        {
                            var absolutePath = Path.Combine(currentDir.Name, targetDir);
                            var newDir = new Directory(absolutePath, currentDir, new List<FileSystemEntry>());
                            allDirs.Add(newDir);
                            currentDir.Children.Add(newDir);
                            currentDir = newDir;
                        }
                    }
                }
                else if (currentCmd == "ls" && !cmd.StartsWith("dir"))
                {
                    var size = cmd.Split(' ')[0].Int();
                    currentDir.Children.Add(new File(size));                   
                }
            }

            return (fileSystemRoot, allDirs);
        }

        private record File(long Size) : FileSystemEntry
        {
            public override long GetSize()
            {
                return Size;
            }
        }

        private record Directory(string Name, Directory? Parent, List<FileSystemEntry> Children) : FileSystemEntry
        {
            public override long GetSize()
            {
                return Children.Sum(x => x.GetSize());
            }
        }

        private abstract record FileSystemEntry()
        {
            public abstract long GetSize();
        }
    }