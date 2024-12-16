using AdventOfCode.Library;

    namespace AdventOfCode._2024;

    public class Day16 : DayEngine
    {
        // public override string[] TestInput =>
        // [
        //     "###############",
        //     "#.......#....E#",
        //     "#.#.###.#.###.#",
        //     "#.....#.#...#.#",
        //     "#.###.#####.#.#",
        //     "#.#.#.......#.#",
        //     "#.#.#####.###.#",
        //     "#...........#.#",
        //     "###.#.#####.#.#",
        //     "#...#.....#.#.#",
        //     "#.#.#.###.#.#.#",
        //     "#.....#...#.#.#",
        //     "#.###.#.#.#.#.#",
        //     "#S..#.....#...#",
        //     "###############"
        // ];
        
        public override string[] TestInput =>
        [
            "#################",
            "#...#...#...#..E#",
            "#.#.#.#.#.#.#.#.#",
            "#.#.#.#...#...#.#",
            "#.#.#.#.###.#.#.#",
            "#...#.#.#.....#.#",
            "#.#.#.#.#.#####.#",
            "#.#...#.#.#.....#",
            "#.#.#####.#.###.#",
            "#.#.#.......#...#",
            "#.#.###.#####.###",
            "#.#.#...#.....#.#",
            "#.#.#.#####.###.#",
            "#.#.#.........#.#",
            "#.#.#.#########.#",
            "#S#.............#",
            "#################",
        ];

        protected override object HandlePart1(string[] input)
        {
            var grid = Grid<char>.Create(input, c => c);

            return Solve(grid);
        }

        protected override object HandlePart2(string[] input)
        {
            var grid = Grid<char>.Create(input, c => c);

            return Solve2(grid);
        }

        int Solve(Grid<char> grid)
        {
            var start = grid.IndexOf('S');
            var end = grid.IndexOf('E');
            var startFacing = new Vector(1, 0);
            
            var visited = new HashSet<Point>();
            var next = new PriorityQueue<(Point, Vector), int>();

            next.Enqueue((start, startFacing), 0);
            
            while (next.TryDequeue(out var ctx, out var score))
            {
                var (point, facing) = ctx;

                if (point == end) return score;
                
                if (!visited.Add(point)) continue;

                foreach (var direction in Vector.GenerateStraightDirectionalForGrid(1, point, grid))
                {
                    var nextPoint = point + direction;
                    var isWall = grid[nextPoint] == '#';
                    var nextScore = (direction == facing) switch
                    {
                        true => score + 1, // if not turning, then we can just move forward 1, which is 1 score
                        false => score + 1001, // turning requires 1000 score.
                    };
                    if (!isWall) next.Enqueue((nextPoint, direction), nextScore);
                }
                
            }

            return -1;
        }
        
        int Solve2(Grid<char> grid)
        {
            var start = grid.IndexOf('S');
            var end = grid.IndexOf('E');
            var startFacing = new Vector(1, 0);
            
            var visited = new Dictionary<(Point, Vector), int>();
            var next = new PriorityQueue<(Point, Vector, LinkedListPoint), int>();

            next.Enqueue((start, startFacing, new LinkedListPoint(null, start)), 0);
            
            while (next.TryDequeue(out var ctx, out var score))
            {
                var (point, facing, prev) = ctx;
                
                if (point == end)
                {
                    var set = new HashSet<Point>() { start, end };
                    prev.Collect(set);
                    while (next.TryDequeue(out var innerCtx, out var innerScore) && innerScore == score)
                    {
                        // check for end
                        if (innerCtx.Item1 == end)
                        {
                            innerCtx.Item3.Collect(set);
                        }
                    }
                    
                    return set.Count;
                }
                
                // Just do a heuristic if we're facing another way than last
                if (visited.TryGetValue((point, facing), out var s) && s < score) continue;
                visited[(point, facing)] = score;

                foreach (var direction in Vector.GenerateStraightDirectionalForGrid(1, point, grid))
                {
                    var nextPoint = point + direction;
                    var isWall = grid[nextPoint] == '#';
                    var nextScore = (direction == facing) switch
                    {
                        true => score + 1, // if not turning, then we can just move forward 1, which is 1 score
                        false => score + 1001, // turning requires 1000 score.
                    };

                    if (visited.TryGetValue((nextPoint, direction), out var currentNextScore) && currentNextScore < nextScore) continue;
                    if (!isWall) next.Enqueue((nextPoint, direction, new LinkedListPoint(prev, nextPoint)), nextScore);
                }
                
            }

            return -1;
        }

        record LinkedListPoint(LinkedListPoint? Prev, Point Point)
        {
            public void Collect(HashSet<Point> into)
            {
                var self = this;
                do
                {
                    into.Add(self.Point);
                    self = self.Prev!;
                } while (self.Prev != null);
            }
        }
    }