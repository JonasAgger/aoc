using AdventOfCode.Library;

    namespace AdventOfCode._2024;

    public class Day6 : DayEngine
    {
        public override string[] TestInput =>
        [
            "....#.....",
            ".........#",
            "..........",
            "..#.......",
            ".......#..",
            "..........",
            ".#..^.....",
            "........#.",
            "#.........",
            "......#..."
        ];

        protected override object HandlePart1(string[] input)
        {
            var grid = Grid<char>.Create(input, c => c);
            var currentPoint = grid.IndexOf('^');
            var visited = Solve(grid, currentPoint);
            
            return visited.Count;
        }

        protected override object HandlePart2(string[] input)
        {
            var grid = Grid<char>.Create(input, c => c);
            var startingPoint = grid.IndexOf('^');
            var visited = Solve(grid, startingPoint);

            visited.Remove(startingPoint);
            return visited.Count(blocker => Loops(grid, startingPoint, blocker));
        }

        private bool Loops(Grid<char> grid, Point startingPoint, Point blocker)
        {
            var movementVector = new Vector(0, -1);
            var visited = new Dictionary<Point, List<Vector>>();
            
            var currentPoint = startingPoint;

            while (grid.IsPointWithinBounds(currentPoint))
            {
                var vectors = visited.GetOrAdd(currentPoint, () => []);
                if (vectors.Contains(movementVector))
                {
                    return true;
                }
                vectors.Add(movementVector);


                var next = currentPoint + movementVector;

                while (grid[next] == '#' || next == blocker)
                {
                    movementVector = movementVector.TurnRight();
                    next = currentPoint + movementVector;

                }

                currentPoint += movementVector;
            }
            
            return false;
        }

        private HashSet<Point> Solve(Grid<char> grid, Point currentPoint)
        {
            var visited = new HashSet<Point> { currentPoint };

            var movementVector = new Vector(0, -1);
            currentPoint += movementVector;

            while (grid.IsPointWithinBounds(currentPoint))
            {
                visited.Add(currentPoint);
                
                var next = currentPoint + movementVector;

                if (grid[next] == '#')
                {
                    movementVector = movementVector.TurnRight();
                }
                
                currentPoint += movementVector;
            }

            return visited;
        }
    }
    
    