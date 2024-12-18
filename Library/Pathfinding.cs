namespace AdventOfCode.Library;

public static class Pathfinding
{
    public static void Djikstra()
    {
        /*
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
         */
    }
}