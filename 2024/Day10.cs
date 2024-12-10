using AdventOfCode.Library;

    namespace AdventOfCode._2024;

    public class Day10 : DayEngine
    {
        // public override string[] TestInput => new string[]
        // {
        //     "0123",
        //     "1234",
        //     "8765",
        //     "9876",
        // };

        public override string[] TestInput => new string[]
        {
            "89010123",
            "78121874",
            "87430965",
            "96549874",
            "45678903",
            "32019012",
            "01329801",
            "10456732",
        };
        
        protected override object HandlePart1(string[] input)
        {
            var grid = Grid<int>.Create(input, c => c - '0');

            var memory = grid.IndexOfAll(9).ToDictionary(x => x, x => new HashSet<Point> {x});
            var starts = grid.IndexOfAll(0);

            return starts.Sum(x => Bfs2(grid, x, memory).Count);
        }

        protected override object HandlePart2(string[] input)
        {
            var grid = Grid<int>.Create(input, c => c - '0');

            var memory = grid.IndexOfAll(9).ToDictionary(x => x, _ => 1);
            var starts = grid.IndexOfAll(0);

            return starts.Sum(x => Bfs(grid, x, memory));
        }

        int Bfs(Grid<int> grid, Point current, Dictionary<Point, int> memory)
        {
            var value = grid[current];
            
            // Console.WriteLine($"{current} -- {value}");

            if (memory.TryGetValue(current, out var count))
            {
                // Console.WriteLine($"Memory: {count}");
                return count;
            }

            var sum = Vector.GenerateStraightDirectionalForGrid(1, current, grid)
                .Select(x => current + x)
                .Where(x => grid[x] == value + 1)
                .Sum(x => Bfs(grid, x, memory));
            
            memory.Add(current, sum);
            return sum;
        }
        
        HashSet<Point> Bfs2(Grid<int> grid, Point current, Dictionary<Point, HashSet<Point>> memory)
        {
            var value = grid[current];
            
            // Console.WriteLine($"{current} -- {value}");

            if (memory.TryGetValue(current, out var count))
            {
                // Console.WriteLine($"Memory: {count}");
                return count;
            }

            var sum = Vector.GenerateStraightDirectionalForGrid(1, current, grid)
                .Select(x => current + x)
                .Where(x => grid[x] == value + 1)
                .SelectMany(x => Bfs2(grid, x, memory))
                .ToHashSet();
            
            memory.Add(current, sum);
            return sum;
        }
    }