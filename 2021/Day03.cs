using AdventOfCode.Library;

namespace AdventOfCode._2021;

public class Day3 : DayEngine
{
    public override string[] TestInput => new string[]
    {
        "00100",
        "11110",
        "10110",
        "10111",
        "10101",
        "01111",
        "00111",
        "11100",
        "10000",
        "11001",
        "00010",
        "01010",
    };

    protected override ValueTask<object> HandlePart1(string[] input)
    {
        var summedBits = input
            .Aggregate(new int[input[0].Length], (counts, s) => s.Select(x => int.Parse(x.ToString())).Select((bit, i) => counts[i] += bit).Select(_ => counts).Last())
            .ToArray();

        var gammaRate = ToInt(summedBits.Select(x => x > input.Length / 2 ? (byte)1 : (byte)0).ToArray());
        var epsilonRate = ToInt(summedBits.Select(x => x < input.Length / 2 ? (byte)1 : (byte)0).ToArray());

        return ValueTask.FromResult<object>(gammaRate * epsilonRate);
    }

    protected override ValueTask<object> HandlePart2(string[] input)
    {
        var oxygenRating = ToInt(GetMostCommon(input));
        var scrubberRating = ToInt(GetLeastCommon(input));

        return ValueTask.FromResult<object>(oxygenRating * scrubberRating);
    }

    private int ToInt(byte[] bits)
    {
        var intValue = 0;
        for (var i = 0; i < bits.Length; i++)
        {
            var bit = bits[i];
            var val = bit << bits.Length - 1 - i;
            intValue |= val;
        }

        return intValue;
    }

    private int ToInt(string str)
    {
        return ToInt(str.Select(x => x == '1' ? (byte) 1 : (byte) 0).ToArray());
    }

    private string GetMostCommon(string[] input)
    {
        var candidates = new List<string>(input);
        var newCandicates = new List<string>();

        var index = 0;

        while (candidates.Count > 1)
        {
            var bit = GetMostCommonBit(candidates.ToArray(), index);
            for (int i = 0; i < candidates.Count; i++)
            {
                var comparand = int.Parse(candidates[i][index].ToString()) == 1;
                if (bit == comparand)
                {
                    newCandicates.Add(candidates[i]);
                }
            }

            candidates = newCandicates;
            newCandicates = new List<string>();
            index++;
        }

        return candidates.First();
    }

    private string GetLeastCommon(string[] input)
    {
        var candidates = new List<string>(input);
        var newCandicates = new List<string>();

        var index = 0;

        while (candidates.Count > 1)
        {
            var bit = !GetMostCommonBit(candidates.ToArray(), index);
            for (int i = 0; i < candidates.Count; i++)
            {
                var comparand = int.Parse(candidates[i][index].ToString()) == 1;
                if (bit == comparand)
                {
                    newCandicates.Add(candidates[i]);
                }
            }

            candidates = newCandicates;
            newCandicates = new List<string>();
            index++;
        }

        return candidates.First();
    }

    private bool GetMostCommonBit(string[] input, int index)
    {
        var count = input.Count(x => x[index] == '1');
        return count >= input.Length / 2.0d;
    }
}