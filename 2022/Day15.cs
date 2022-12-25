using System.Text.RegularExpressions;
using AdventOfCode.Library;

    namespace AdventOfCode._2022;

    public class Day15 : DayEngine
    {
        public override string[] TestInput => new string[]
        {
            "Sensor at x=2, y=18: closest beacon is at x=-2, y=15",
            "Sensor at x=9, y=16: closest beacon is at x=10, y=16",
            "Sensor at x=13, y=2: closest beacon is at x=15, y=3",
            "Sensor at x=12, y=14: closest beacon is at x=10, y=16",
            "Sensor at x=10, y=20: closest beacon is at x=10, y=16",
            "Sensor at x=14, y=17: closest beacon is at x=10, y=16",
            "Sensor at x=8, y=7: closest beacon is at x=2, y=10",
            "Sensor at x=2, y=0: closest beacon is at x=2, y=10",
            "Sensor at x=0, y=11: closest beacon is at x=2, y=10",
            "Sensor at x=20, y=14: closest beacon is at x=25, y=17",
            "Sensor at x=17, y=20: closest beacon is at x=21, y=22",
            "Sensor at x=16, y=7: closest beacon is at x=15, y=3",
            "Sensor at x=14, y=3: closest beacon is at x=15, y=3",
            "Sensor at x=20, y=1: closest beacon is at x=15, y=3",
        };


        protected override object HandlePart1(string[] input)
        {
            var row = 2000000;
            // var row = 10;

            var points = input
                .Select(Parse)
                .Where(x =>
            {
                var closest = new Point(x.X, row);

                return x.Value >= x.AsPoint().ManhattanDistance(closest);
            }).ToList();


            var ranges = points.Select(x =>
            {
                var closest = new Point(x.X, row);
                var md = x.AsPoint().ManhattanDistance(closest);

                var distanceDiff = x.Value - md;

                return (new Point(x.X - distanceDiff, row), new Point(x.X + distanceDiff, row));
            })
                .OrderBy(x => x.Item1.X)
                .ToList();

            var x = ranges.First().Item1.X;

            var count = 0;

            foreach (var (first, last) in ranges)
            {
                var current = Math.Max(x, first.X);
                var end = last.X;
                while (current < end)
                {
                    count++;
                    current++;
                }

                x = current;
            }
            
            return count;
        }

        protected override object HandlePart2(string[] input)
        {
            var maxCoord = 4_000_000;
            // var maxCoord = 20;
            
            var rombs = input.Select(Parse2).ToList();

            var squares = new List<Square>() { new Square(new Point(0, 0), maxCoord) };
            var result = new List<Square>();
            
            while (true)
            {
                foreach (var square in squares)
                {
                    var subsquares = square.CreateSubSquares(maxCoord, maxCoord > 100 ? 100 : 4);

                    foreach (var subsquare in subsquares)
                    {
                        if (!rombs.Any(x => x.FullyCovers(subsquare)))
                        {
                            result.Add(subsquare);
                        }
                    }
                }

                if (result.Count == 1) break;

                squares = result;
                result = new();
                maxCoord = maxCoord > 100 ? maxCoord / 100 : maxCoord / 4;
            }

            var p = result.First().Origin;
            return ((long)p.X) * 4_000_000L + p.Y;
        }

        private Point<int> Parse(string str)
        {
            var re = new Regex(@"Sensor at x=([\-\d]+), y=([\-\d]+): closest beacon is at x=([\-\d]+), y=([\-\d]+)");

            var m = re.Match(str);
            
            var x1 = m.Groups[1].Value.Int();
            var y1 = m.Groups[2].Value.Int();
            var x2 = m.Groups[3].Value.Int();
            var y2 = m.Groups[4].Value.Int();

            var p1 = new Point(x1, y1);
            var p2 = new Point(x2, y2);

            return p1.Value(p1.ManhattanDistance(p2));
        }
        
        private Romb Parse2(string str)
        {
            var re = new Regex(@"Sensor at x=([\-\d]+), y=([\-\d]+): closest beacon is at x=([\-\d]+), y=([\-\d]+)");

            var m = re.Match(str);
            
            var x1 = m.Groups[1].Value.Int();
            var y1 = m.Groups[2].Value.Int();
            var x2 = m.Groups[3].Value.Int();
            var y2 = m.Groups[4].Value.Int();

            var p1 = new Point(x1, y1);
            var p2 = new Point(x2, y2);

            return new Romb(p1, p2);
        }

        private record Romb(Point P1, Point P2)
        {
            public int Distance { get; } = P1.ManhattanDistance(P2);

            public bool FullyCovers(Square subsquare)
            {
                return IsPointWithinRhomb(subsquare.P1) &&
                       IsPointWithinRhomb(subsquare.P2) &&
                       IsPointWithinRhomb(subsquare.P3) &&
                       IsPointWithinRhomb(subsquare.P4);
            }
            
            private bool IsPointWithinRhomb(Point p) {
                return P1.ManhattanDistance(p) <= Distance;
            }
        }

        private record Square(Point Origin, int Side)
        {
            public Point P1 { get; } = Origin;
            public Point P2 { get; } = Origin + new Point(Side, 0);
            public Point P3 { get; } = Origin + new Point(0, Side);
            public Point P4 { get; } = Origin + new Point(Side, Side);
            
            public List<Square> CreateSubSquares(int overallLenght, int divideFactor) {
                var res = new List<Square>();

                var squareLength = overallLenght / divideFactor;

                for (var x = Origin.X; x < Origin.X + overallLenght; x += squareLength) {
                    for (var y= Origin.Y; y < Origin.Y + overallLenght; y += squareLength) {
                        res.Add(new Square(new Point() {X=x, Y=y}, squareLength - 1));
                    }
                }

                return res;
            }
        }
    }