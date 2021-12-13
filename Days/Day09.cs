using AdventOfCode.Library;

namespace AdventOfCode.Days;

public class Day9 : DayEngine
{
    public override string[] TestInput => new string[]
    {
        "2199943210",
        "3987894921",
        "9856789892",
        "8767896789",
        "9899965678",
    };

    protected override ValueTask<object> HandlePart1(string[] input)
    {
        var grid = Grid<int>.Create(input, x => int.Parse(x.ToString()));

        var summedLowValues = FetchLowPoints(grid)
            .Select(point => grid[point.Y, point.X] + 1)
            .Sum();
        
        return ValueTask.FromResult<object>(summedLowValues);
    }

    protected override ValueTask<object> HandlePart2(string[] input)
    {
        var grid = Grid<int>.Create(input, x => int.Parse(x.ToString()));
        
        var threeLargestBasinsMultiplied = FetchLowPoints(grid)
            .Select(point => FetchBasinSize(grid, point))
            .OrderByDescending(x => x)
            .Take(3)
            .Aggregate(1, (accu, curr) => accu * curr);
        
        return ValueTask.FromResult<object>(threeLargestBasinsMultiplied);
    }

    private int FetchBasinSize(Grid<int> grid, Point origin)
    {
        return WalkUp(grid, origin, new HashSet<Point>()).Count();
    }

    private IEnumerable<Point> WalkUp(Grid<int> grid, Point origin, HashSet<Point> pointsSeen)
    {
        const int BlockingValue = 9;
        var originValue = grid[origin.Y, origin.X];
        
        if (!pointsSeen.Add(origin)) yield break;
        
        if (grid.IsYValueWithinBounds(origin.Y - 1))
        {
            var value = grid[origin.Y - 1, origin.X];
            if (value != BlockingValue && value > originValue)
            {
                foreach (var point in WalkUp(grid, new Point(origin.X, origin.Y - 1), pointsSeen))
                {
                    yield return point;
                }
            }
        }
        if (grid.IsYValueWithinBounds(origin.Y + 1))
        {
            var value = grid[origin.Y + 1, origin.X];
            if (value != BlockingValue && value > originValue)
            {
                foreach (var point in WalkUp(grid, new Point(origin.X, origin.Y + 1), pointsSeen))
                {
                    yield return point;
                }
            }
        }
        if (grid.IsXValueWithinBounds(origin.X - 1))
        {
            var value = grid[origin.Y, origin.X - 1];
            if (value != BlockingValue && value > originValue)
            {
                foreach (var point in WalkUp(grid, new Point(origin.X - 1, origin.Y), pointsSeen))
                {
                    yield return point;
                }
            }
        }
        if (grid.IsXValueWithinBounds(origin.X + 1))
        {
            var value = grid[origin.Y, origin.X + 1];
            if (value != BlockingValue && value > originValue)
            {
                foreach (var point in WalkUp(grid, new Point(origin.X + 1, origin.Y), pointsSeen))
                {
                    yield return point;
                }
            }
        }

        yield return origin;
    }

    private IEnumerable<Point> FetchLowPoints(Grid<int> grid)
    {
        for (int row = 0; row < grid.Size; row++)
        {
            for (int col = 0; col < grid.Size; col++)
            {
                if (IsLocalLow(grid, col, row))
                {
                    yield return new Point(col, row);
                }
            }
        }
    }

    private bool IsLocalLow(Grid<int> grid, int x, int y)
    {
        var lowValue = int.MaxValue;
        var index = 0;
        var lowIndex = -1;

        for (int i = y - 1; i < y + 2; i++)
        {
            for (int j = x - 1; j < x + 2; j++)
            {
                var localValue = grid[y, x];

                if (lowValue > localValue)
                {
                    lowValue = localValue;
                    lowIndex = index;
                }

                index += 1;
            }
        }

        return lowIndex == 4;
    }

    private readonly record struct Point(int X, int Y);
}