using AdventOfCode.Library;

    namespace AdventOfCode._2024;

    public class Day20 : DayEngine
    {
        private static Variable<int> SaveAtLeast = Variable<int>.Create(39, 100);
        private static Variable<int> SaveAtLeast2 = Variable<int>.Create(74, 100);
        
        public override string[] TestInput => new string[]
        {
            "###############",
            "#...#...#.....#",
            "#.#.#.#.#.###.#",
            "#S#...#.#.#...#",
            "#######.#.#.###",
            "#######.#.#...#",
            "#######.#.###.#",
            "###..E#...#...#",
            "###.#######.###",
            "#...###...#...#",
            "#.#####.#.###.#",
            "#.#...#.#.#...#",
            "#.#.#.#.#.#.###",
            "#...#...#...###",
            "###############",
        };
        

        protected override object HandlePart1(string[] input)
        {
            var maze = Prepare(input);
            return maze.Solve(2, SaveAtLeast);
        }

        protected override object HandlePart2(string[] input)
        {
            var maze = Prepare(input);
            return maze.Solve(20, SaveAtLeast2);
        }

        Maze Prepare(string[] input)
        {
            var grid = Grid<int>.Create(input, c => c switch
            {
                '#' => -1,
                '.' => 0,
                'S' => -2,
                'E' => -3,
            });
            var start = grid.IndexOf(-2);
            var end = grid.IndexOf(-3);
            var path = new List<Point>() { start };
            grid[end] = 0;

            var current = start;
            var index = 0;
            while (current != end)
            {
                current = Vector
                    .GenerateStraightDirectionalForGrid(1, current, grid)
                    .Select(x => current + x)
                    .First(x => grid[x] == 0);
                index += 1;
                grid[current] = index;
                path.Add(current);
            }

            grid[start] = 0;

            return new Maze(grid, path);
        }
    }

    record Maze(Grid<int> Grid, List<Point> Path)
    {
        public int Solve(int maxSkip, int saveAtLeast)
        {
            var count = 0;
            for (int i = 0; i < Path.Count - saveAtLeast; i++)
            {
                var current = Path[i];
                var target = i + saveAtLeast;
                foreach (var targetPoint in Path[target..])
                {
                    var distance = targetPoint.ManhattanDistance(current);
                    var valueDistance = Grid[targetPoint] - Grid[current] - distance;
                    if (distance <= maxSkip && valueDistance >= saveAtLeast)
                    {
                        count++;
                    }
                }
            }

            return count;
        }
    }