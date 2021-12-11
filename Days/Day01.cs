using AdventOfCode.Library;

namespace AdventOfCode.Days;

public class Day1 : DayEngine
{
    public override string[] TestInput => new[]
    {
        "199",
        "200",
        "208",
        "210",
        "200",
        "207",
        "240",
        "269",
        "260",
        "263",
    };

    protected override Task<object> HandlePart1(string[] input)
    {
        var ints = input.Select(int.Parse).ToArray();

        var sum = 0;

        for (int i = 1; i < ints.Length; i++)
        {
            if (ints[i-1] < ints[i]) sum++;
        }

        return Task.FromResult<object>(sum);
    }

    protected override Task<object> HandlePart2(string[] input)
    {
        var ints = input.Select(int.Parse).ToArray();

        var windows = new int[ints.Length - 2];

        for (int i = 0; i < ints.Length - 2; i++)
        {
            windows[i] = 0;
            for (int j = i; j < i + 3; j++)
            {
                windows[i] += ints[j];
            }
        }

        var sum = 0;

        for (int i = 1; i < windows.Length; i++)
        {
            if (windows[i - 1] < windows[i]) sum++;
        }

        return Task.FromResult<object>(sum);
    }
}