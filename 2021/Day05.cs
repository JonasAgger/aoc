using AdventOfCode.Library;

namespace AdventOfCode._2021;

public class Day5 : DayEngine
{
    public override string[] TestInput => new string[]
    {
        "0,9 -> 5,9",
        "8,0 -> 0,8",
        "9,4 -> 3,4",
        "2,2 -> 2,1",
        "7,0 -> 7,4",
        "6,4 -> 2,0",
        "0,9 -> 2,9",
        "3,4 -> 1,4",
        "0,0 -> 8,8",
        "5,5 -> 8,2",
    };

    protected override ValueTask<object> HandlePart1(string[] input)
    {
        var lines = input.Select(x => new Line(x)).ToArray();

        var max = lines.Max(x => x.Max) + 1;

        var board = new int[max][];
        for (int i = 0; i < max; i++)
        {
            board[i] = new int[max];
        }

        foreach (var line in lines.Where(x => !x.IsDiagonal()))
        {
            foreach (var point in line.Travel())
                board[point.y][point.x] += 1;
        }

        var overlappingPoints = board.SelectMany(x => x).Count(x => x > 1);
        
        return ValueTask.FromResult<object>(overlappingPoints);
    }

    protected override ValueTask<object> HandlePart2(string[] input)
    {
        var lines = input.Select(x => new Line(x)).ToArray();

        var max = lines.Max(x => x.Max) + 1;

        var board = new int[max][];
        for (int i = 0; i < max; i++)
        {
            board[i] = new int[max];
        }

        foreach (var line in lines)
        {
            foreach (var point in line.Travel())
                board[point.y][point.x] += 1;
        }

        var overlappingPoints = board.SelectMany(x => x).Count(x => x > 1);
        
        return ValueTask.FromResult<object>(overlappingPoints);
    }

    private class Line
    {
        public Line(string str)
        {
            var ints = str.Split("->").SelectMany(x => x.Split(',')).Select(int.Parse).ToArray();
            X1 = ints[0];
            Y1 = ints[1];
            X2 = ints[2];
            Y2 = ints[3];
            Max = ints.Max();
        }
        public int Max { get; set; }
        public int X1 { get; set; }
        public int Y1 { get; set; }
        public int X2 { get; set; }
        public int Y2 { get; set; }

        public bool IsDiagonal()
        {
            return !(X1 == X2 || Y1 == Y2);
        }

        public IEnumerable<(int x, int y)> Travel()
        {
            var deltaX = X1 < X2 ? 1 : X2 < X1 ? -1 : 0;
            var deltaY = Y1 < Y2 ? 1 : Y2 < Y1 ? -1 : 0;

            var x = X1;
            var y = Y1;
            
            yield return (x, y);
            
            do
            {
                x += deltaX;
                y += deltaY;
                yield return (x, y);
            } while (x != X2 || y != Y2);
        }
    }
}