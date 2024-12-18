using AdventOfCode.Library;

    namespace AdventOfCode._2024;

    public class Day18 : DayEngine
    {
        private static readonly Variable<int> GridSize = Variable<int>.Create(6, 70);
        private static readonly Variable<int> Steps = Variable<int>.Create(12, 1024);
        
        public override string[] TestInput =>
        [
            "5,4",
            "4,2",
            "4,5",
            "3,0",
            "2,1",
            "6,3",
            "2,4",
            "1,5",
            "0,6",
            "3,3",
            "2,6",
            "5,1",
            "1,2",
            "5,5",
            "2,5",
            "6,5",
            "1,4",
            "0,4",
            "6,4",
            "1,1",
            "6,1",
            "1,0",
            "0,5",
            "1,6",
            "2,0"
        ];

        protected override object HandlePart1(string[] input)
        {
            var grid = Grid<bool>.Create(GridSize + 1, GridSize + 1); // Bounds are including
            foreach (var point in input.Select(x =>
                     {
                         var parts = x.Split(',');
                         return new Point(parts[0].Int(), parts[1].Int());
                     }).Take(Steps))
            {
                grid[point] = true;
            }
            
            return Solve(grid).Item1;
        }

        protected override object HandlePart2(string[] input)
        {
            var points = input.Select(x =>
            {
                var parts = x.Split(',');
                return new Point(parts[0].Int(), parts[1].Int());
            }).ToList();
            
            var grid = Grid<bool>.Create(GridSize + 1, GridSize + 1); // Bounds are including
            foreach (var point in points.Take(Steps))
            {
                grid[point] = true;
            }

            var bestPath = Solve(grid).Item2.Collect();

            foreach (var point in points[(int)Steps..])
            {
                grid[point] = true;
                if (!bestPath.Contains(point)) continue;
                
                var (score, path) = Solve(grid);
                if (score == -1) return point;
                bestPath = path.Collect();
            }
            
            return null;
        }
        
        (int, LinkedListPoint) Solve(Grid<bool> grid)
        {
            var start = new Point(0,0);
            var end = new Point(GridSize, GridSize);
            
            var visited = new HashSet<Point>();
            var next = new PriorityQueue<LinkedListPoint, int>();

            next.Enqueue(new LinkedListPoint(null, start), 0);
            
            while (next.TryDequeue(out var points, out var score))
            {
                var currentPoint = points.Point;
                if (currentPoint == end) return (score, points);
                
                if (!visited.Add(currentPoint)) continue;

                foreach (var direction in Vector.GenerateStraightDirectionalForGrid(1, currentPoint, grid))
                {
                    var nextPoint = currentPoint + direction;
                    var isWall = grid[nextPoint];
                    var nextScore = score + 1;
                    if (!isWall) next.Enqueue(points.Next(nextPoint), nextScore);
                }
                
            }

            return (-1, new LinkedListPoint(null, Point.None));
        }

        record LinkedListPoint(LinkedListPoint? Prev, Point Point)
        {
            private int length = 0;

            public LinkedListPoint Next(Point p)
            {
                return new LinkedListPoint(this, p)
                {
                    length = this.length + 1,
                };
            }

            public HashSet<Point> Collect()
            {
                var set = new HashSet<Point>(); 
                var self = this;
                do
                {
                    set.Add(self.Point);
                    self = self.Prev!;
                } while (self.Prev != null);

                set.Add(self.Point);
                return set;
            }

            public int Length() => length;
        }
    }