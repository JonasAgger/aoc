using System.Diagnostics;
using AdventOfCode.Library;

    namespace AdventOfCode._2022;

    public class Day12 : DayEngine
    {
        public override string[] TestInput => new string[]
        {
            "Sabqponm",
            "abcryxxl",
            "accszExk",
            "acctuvwj",
            "abdefghi",
        };

        protected override object HandlePart1(string[] input)
        {
            var grid = Grid<int>.Create(input, c =>
            {
                if (char.IsLower(c))
                    return c - 'a';

                if (c == 'S') return -1;
                return -2;
            });

            var start = grid.IndexOf(-1);
            var end = grid.IndexOf(-2);

            grid[start] = 0;
            grid[end] = 'z' - 'a';

            return Walk(start, end, grid);
        }

        protected override object HandlePart2(string[] input)
        {
            var grid = Grid<int>.Create(input, c =>
            {
                if (char.IsLower(c))
                    return c - 'a';

                if (c == 'S') return 0;
                return -2;
            });

            var starts = grid.IndexOfAll(0);
            var end = grid.IndexOf(-2);
            
            
            return starts.Min(x => Walk(x, end, grid));
        }


        
        
        private int Walk(Point start, Point end, Grid<int> grid)
        {
            var visited = new HashSet<Point>();
            visited.Add(start);
            
            
            var prioQueue = new PriorityQueue<Point, int>();
            prioQueue.Enqueue(start, 0);
            

            while (prioQueue.TryDequeue(out var point, out var prio))
            {
                if (point == end) return prio;

                foreach (var p in GetSurrondings(point, grid, visited))
                {
                    visited.Add(p);
                    prioQueue.Enqueue(p, prio + 1);
                }
            }

            return int.MaxValue;
        }

        private readonly Point[] adjacencyMatrix = new[]
        {
            new Point(0, 1),
            new Point(0, -1),
            new Point(1, 0),
            new Point(-1, 0),
        };

        private IEnumerable<Point> GetSurrondings(Point p, Grid<int> grid, HashSet<Point> hashSet)
        {
            return adjacencyMatrix
                .Select(x => p + x)
                .Where(grid.IsPointWithinBounds)
                .Where(x => !hashSet.Contains(x))
                .Where(x =>
                {
                    var currentElevation = grid[p];
                    var newElevation = grid[x];

                    return newElevation - currentElevation < 2;
                });
        }
    }