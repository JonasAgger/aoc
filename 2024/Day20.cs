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
            grid[end] = 0; // Fix for the dumb pathfinding below

            var current = start;
            var index = 0; // To track how many steps we are along.
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
            return Path
                .SkipLast(saveAtLeast) // We can skip the last 'n' given that even if we teleport to end it cant skip enough.
                .Select(((current, i) =>
                {
                    // since we need to save at least 'n' and the Path is linear, with every move being equally expensive,
                    // then saving at least 'n' just requires us to lookahead at least 'n'
                    
                    var target = i + saveAtLeast;
                    return Path.Skip(target).Count(targetPoint =>
                    {
                        // After that we just need to check that the value distance between us and the target is greater than 'n'
                        // We need to factor in the manhattan distance as well, given that we also have to move to the given point.
                        var distance = targetPoint.ManhattanDistance(current);
                        var valueDistance = Grid[targetPoint] - Grid[current] - distance;
                        return distance <= maxSkip && valueDistance >= saveAtLeast;
                    });
                }))
                .Sum();
        }
    }