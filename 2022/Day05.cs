using AdventOfCode.Library;

    namespace AdventOfCode._2022;

    public class Day5 : DayEngine
    {
        public override string[] TestInput => new string[]
        {
            "    [D]    ",
            "[N] [C]    ",
            "[Z] [M] [P]",
            " 1   2   3 ",
            "move 1 from 2 to 1",
            "move 3 from 1 to 3",
            "move 2 from 2 to 1",
            "move 1 from 1 to 2",
        };

        protected override object HandlePart1(string[] input)
        {
            var (cratesLine, deques) = Init(input);
            
            for (int i = cratesLine + 1; i < input.Length; i++)
            {
                var instructions = input[i].Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                var count = instructions[1].Int();
                var from = instructions[3].Int();
                var to = instructions[5].Int();

                for (int j = 0; j < count; j++)
                {
                    var entry = deques[from - 1].Pop();
                    if (entry != default) deques[to - 1].Add(entry);
                }
            }
            
            return new string(deques.Select(x => x.Pop()).ToArray());
        }

        protected override object HandlePart2(string[] input)
        {
            var (cratesLine, deques) = Init(input);
            
            for (int i = cratesLine + 1; i < input.Length; i++)
            {
                var instructions = input[i].Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                var count = instructions[1].Int();
                var from = instructions[3].Int();
                var to = instructions[5].Int();

                var movingBlocks = new char[count];
                for (int j = 0; j < count; j++)
                {
                    var entry = deques[from - 1].Pop();
                    movingBlocks[j] = entry;
                }
                for (int j = count - 1; j >= 0; j--)
                {
                    deques[to - 1].Add(movingBlocks[j]);
                }
            }
            
            return new string(deques.Select(x => x.Pop()).ToArray());
        }
        
        private (int cratesLine, List<Deque<char>> deques) Init(string[] input)
        {
            var cratesLine = input.IndexOf(x => x.StartsWith(" 1"));
            var deques = input[cratesLine]
                .Trim()
                .Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                .Select(_ => new Deque<char>()).ToList();

            for (int i = cratesLine - 1; i >= 0; i--)
            {
                var index = 0;
                var line = input[i];
                while (line.IndexOf('[', index) != -1)
                {
                    index = line.IndexOf('[', index);
                    var dequeIndex = index / 4;
                    var character = line[++index];
                    deques[dequeIndex].Add(character);
                }
            }

            return (cratesLine, deques);
        }
    }