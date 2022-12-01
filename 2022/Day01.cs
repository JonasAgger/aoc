using AdventOfCode.Library;

namespace AdventOfCode._2022;

public class Day1 : DayEngine
{
    public override string[] TestInput => new[]
    {
        "1000",
        "2000",
        "3000",
        "",
        "4000",
        "",
        "5000",
        "6000",
        "",
        "7000",
        "8000",
        "9000",
        "",
        "10000",
    };
    
    protected override string[] Transform(string str) => str.Split("\n");

    protected override ValueTask<object> HandlePart1(string[] input)
    {
        var elfs = new List<int>() {0};

        foreach (var i in input)
        {
            if (string.IsNullOrEmpty(i)) elfs.Add(0);
            else
            {
                elfs[elfs.Count - 1] += int.Parse(i);
            }
        }

        return new ValueTask<object>(elfs.Max());
    }

    protected override ValueTask<object> HandlePart2(string[] input)
    {
        var elfs = new List<int>() {0};

        foreach (var i in input)
        {
            if (string.IsNullOrEmpty(i)) elfs.Add(0);
            else
            {
                elfs[elfs.Count - 1] += int.Parse(i);
            }
        }

        return new ValueTask<object>(elfs.OrderDescending().Take(3).Sum());
    }
}

