using AdventOfCode.Library;

    namespace AdventOfCode._2022;

    public class Day14 : DayEngine
    {
        public override string[] TestInput => new string[]
        {
            "498,4 -> 498,6 -> 496,6",
            "503,4 -> 502,4 -> 502,9 -> 494,9"
        };

        protected override object HandlePart1(string[] input)
        {
            var graphs = input.Select(x => x.Split("->")
                    .Select(y => y.Split(','))
                    .Select(x => new Point(x[0].Int(), x[1].Int())).ToArray())
                .ToArray();

            var maxX = graphs.Max(x => x.Max(y => y.X)) + 1;
            var maxY = graphs.Max(x => x.Max(y => y.Y)) + 1;

            var grid = Grid<bool>.Create(maxX, maxY);

            foreach (var graph in graphs)
            {
                Populate(graph, grid);
            }

            var c = 0;
            while (SpawnSand(grid))
            {
                c += 1;
            }
            
            return c;
        }

        protected override object HandlePart2(string[] input)
        {
            var graphs = input.Select(x => x.Split("->")
                    .Select(y => y.Split(','))
                    .Select(x => new Point(x[0].Int(), x[1].Int())).ToArray())
                .ToArray();

            var maxX = graphs.Max(x => x.Max(y => y.X)) + 1;
            var maxY = graphs.Max(x => x.Max(y => y.Y)) + 1;

            maxY = maxY + 2;
            maxX = maxX + 500;
            
            var grid = Grid<bool>.Create(maxX, maxY);

            
            Populate(new []
            {
                new Point(0, maxY - 1),
                new Point(maxX - 1, maxY - 1)
            }, grid);
            

            foreach (var graph in graphs)
            {
                Populate(graph, grid);
            }
            
            var c = 0;
            while (SpawnSand(grid))
            {
                c += 1;
            }
            
            return c;
        }

        private void Populate(Point[] graph, Grid<bool> grid)
        {
            for (int i = 0; i < graph.Length - 1; i++)
            {
                var delta = (graph[i + 1] - graph[i]);
                var magnitude = delta.Magnitude();
                
                for (int j = 0; j <= magnitude; j++)
                {
                    var c = graph[i] + (delta.Normalize() * j);
                    grid[c] = true;
                }
            }
        }

        private bool SpawnSand(Grid<bool> grid)
        {
            var p = new Point(500, 0);
            
            while (grid.IsPointWithinBounds(p))
            {
                var down = p + new Point(0, 1);
                var left = p + new Point(-1, 1);
                var right = p + new Point(1, 1);

                if (grid.Size <= p.Y) return false;
                if (!grid[down]) p = down;
                else if (!grid[left]) p = left;
                else if (!grid[right]) p = right;
                else if(!grid[p])
                {
                    grid[p] = true;
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }
    }