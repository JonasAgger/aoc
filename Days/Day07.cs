using AdventOfCode.Library;

namespace AdventOfCode.Days;

public class Day7 : DayEngine
{
    public override string[] TestInput => new string[]
    {
        "16,1,2,0,4,2,7,1,2,14"
    };

    protected override Task<object> HandlePart1(string[] input)
    {
        var ints = input[0].Split(',').Select(int.Parse).OrderBy(x => x).ToArray();

        var max = ints.Max();

        var distances = new int[max+1];

        foreach (var i in ints)
        {
            for (int j = 0; j < max+1; j++)
            {
                distances[j] += Math.Abs(i - j);
            }
        }

        var min = -1;
        var minValue = int.MaxValue;

        for (int i = 0; i < max+1; i++)
        {
            if (minValue > distances[i])
            {
                minValue = distances[i];
                min = i;
            }
        }

        return Task.FromResult<object>(minValue);
    }

    protected override Task<object> HandlePart2(string[] input)
    {
        var ints = input[0].Split(',').Select(int.Parse).OrderBy(x => x).ToArray();

        var max = ints.Max();

        var distances = new int[max + 1];

        foreach (var i in ints)
        {
            for (int j = 0; j < max + 1; j++)
            {
                var x = Math.Abs(i - j);
                distances[j] += x*(x+1)/2;
            }
        }

        var min = -1;
        var minValue = int.MaxValue;

        for (int i = 0; i < max + 1; i++)
        {
            if (minValue > distances[i])
            {
                minValue = distances[i];
                min = i;
            }
        }

        return Task.FromResult<object>(minValue);
    }
}