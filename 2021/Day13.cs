using AdventOfCode.Library;

namespace AdventOfCode._2021;

public class Day13 : DayEngine
{
    public override string[] TestInput => new string[]
    {
        "6,10",
        "0,14",
        "9,10",
        "0,3",
        "10,4",
        "4,11",
        "6,0",
        "6,12",
        "4,1",
        "0,13",
        "10,12",
        "3,4",
        "3,0",
        "8,4",
        "1,10",
        "2,14",
        "8,10",
        "9,0",
        "fold along y=7",
        "fold along x=5",
    };

    protected override ValueTask<object> HandlePart1(string[] input)
    {
        var points = input.TakeWhile(x => !x.StartsWith("fold")).ToArray();
        var folds = input.SkipWhile(x => !x.StartsWith("fold"))
            .Select(x => x.Split(' ', StringSplitOptions.RemoveEmptyEntries).Last())
            .Select(x => (x.Split('=')[0], int.Parse(x.Split('=')[1])))
            .ToArray();

        var grid = Grid<bool>.Create(points, x => new Point<bool>
        {
            X = int.Parse(x.Split(',')[0]),
            Y = int.Parse(x.Split(',')[1]),
            Value = true
        });

        var rawGrid = grid.GetRaw();

        Console.WriteLine($"Size: {rawGrid.Length},{rawGrid[0].Length}");

        foreach (var fold in folds)
        {
            if (fold.Item1 == "y")
            {
                rawGrid = YFold(rawGrid, fold.Item2);
            }
            else
            {
                rawGrid = XFold(rawGrid, fold.Item2);
            }

            Console.WriteLine($"Size: {rawGrid.Length},{rawGrid[0].Length}");
        }

        //Print(rawGrid);

        return ValueTask.FromResult<object>(rawGrid.SelectMany(x => x).Count(x => !x));
    }

    protected override ValueTask<object> HandlePart2(string[] input)
    {

        return ValueTask.FromResult<object>(null);
    }

    private bool[][] YFold(bool[][] grid, int foldIndex)
    {
        var newGrid = new bool[foldIndex][];

        Array.Copy(grid, newGrid, foldIndex);

        for (int i = 0; i < foldIndex; i++)
        {
            for (int j = 0; j < newGrid[0].Length; j++)
            {
                var targetIndex = foldIndex - i - 1;
                if (targetIndex < 0) continue;

                var sourceIndex = foldIndex + i + 1;
                if (sourceIndex >= grid.Length) continue;

                try
                {
                    newGrid[targetIndex][j] |= grid[sourceIndex][j];
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        return newGrid;
    }

    private bool[][] XFold(bool[][] grid, int foldIndex)
    {
        var newGrid = new bool[grid.Length][];
        
        for (int i = 0; i < grid.Length; i++)
        {
            newGrid[i] = new bool[foldIndex];

            for (int j = 0; j < foldIndex; j++)
            {
                var sourceIndex = grid[i].Length - j - 1;
                if (sourceIndex >= grid[0].Length) continue;

                newGrid[i][j] |= grid[i][sourceIndex] || grid[i][j];
            }
        }

        return newGrid;
    }

    private void Print(bool[][] grid)
    {
        for (int i = 0; i < grid.Length; i++)
        {
            Console.WriteLine(string.Join("", grid[i].Select(x => x ? "#" : ".")));
        }

        Console.WriteLine();
        Console.WriteLine();
    }
}