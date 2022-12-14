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

            var maxX = graphs.Max(x => x.Max(y => y.X));
            var maxY = graphs.Max(x => x.Max(y => y.Y));

            var grid = Grid<bool>.Create(maxX, maxY);

            

            return null;
        }

        protected override object HandlePart2(string[] input)
        {
            
            return null;
        }
    }