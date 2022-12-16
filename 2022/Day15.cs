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
            var points = input.Select(Parse).ToArray();
            var row = 10;

            var closest = points.MinBy(x => Math.Abs(x.Y - row)).X;

            var start = new Point(closest, row);
            
            // binary search lowest distance for row. 
            // For every point.
            // Save places where we are within distance.
            
            return points.Length;
        }

        protected override object HandlePart2(string[] input)
        {

            return null;
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
    }