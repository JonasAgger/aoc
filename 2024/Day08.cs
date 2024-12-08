using AdventOfCode.Library;

    namespace AdventOfCode._2024;

    public class Day8 : DayEngine
    {
        public override string[] TestInput =>
        [
            "............",
            "........0...",
            ".....0......",
            ".......0....",
            "....0.......",
            "......A.....",
            "............",
            "............",
            "........A...",
            ".........A..",
            "............",
            "............"
        ];

        
        protected override object HandlePart1(string[] input)
        {
            var grid = Grid<char>.Create(input, c => c);
            var antennas = grid.GetRaw()
                .SelectMany(x => x)
                .Where(x => x != '.')
                .Distinct()
                .Select(x => new Antenna(x, grid.IndexOfAll(x)))
                .ToList();

            var locations = new HashSet<Point>();

            foreach (var antenna in antennas)
            {
                antenna.AntiNodes(grid, locations);
            }

            return locations.Count;
        }

        protected override object HandlePart2(string[] input)
        {
            var grid = Grid<char>.Create(input, c => c);
            var antennas = grid.GetRaw()
                .SelectMany(x => x)
                .Where(x => x != '.')
                .Distinct()
                .Select(x => new Antenna(x, grid.IndexOfAll(x)))
                .ToList();

            var locations = new HashSet<Point>();

            foreach (var antenna in antennas)
            {
                antenna.AntiNodes2(grid, locations);
            }

            return locations.Count;
        }
    }

    record Antenna(char Code, List<Point> Points)
    {
        public void AntiNodes(Grid<char> grid, HashSet<Point> locations)
        {
            for (int i = 0; i < Points.Count - 1; i++)
            {
                var origin = Points[i];

                for (int j = i + 1; j < Points.Count; j++)
                {
                    var target = Points[j];
                    
                    var diff = origin - target;
                    
                    var loc = origin + diff;
                    if (grid.IsPointWithinBounds(loc)) locations.Add(loc);
                    
                    loc = target - diff;
                    if (grid.IsPointWithinBounds(loc)) locations.Add(loc);
                }
            }
        }
        
        public void AntiNodes2(Grid<char> grid, HashSet<Point> locations)
        {
            for (int i = 0; i < Points.Count - 1; i++)
            {
                var origin = Points[i];

                for (int j = i + 1; j < Points.Count; j++)
                {
                    var target = Points[j];
                    
                    var diff = origin - target;

                    locations.Add(origin);
                    locations.Add(target);

                    var loc = origin + diff;
                    while (grid.IsPointWithinBounds(loc))
                    {
                        locations.Add(loc);
                        loc += diff;
                    }
                    
                    loc = target - diff;
                    while (grid.IsPointWithinBounds(loc))
                    {
                        locations.Add(loc);
                        loc -= diff;
                    }
                }
            }
        }
    }