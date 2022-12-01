using AdventOfCode.Library;

namespace AdventOfCode._2021;

public class Day8 : DayEngine
{
    public override string[] TestInput => new string[]
    {
        // "acedgfb cdfbe gcdfa fbcad dab cefabd cdfgeb eafb cagedb ab | cdfeb fcadb cdfeb cdbaf",
        
        "be cfbegad cbdgef fgaecd cgeb fdcge agebfd fecdb fabcd edb | fdgacbe cefdb cefbgd gcbe",
        "edbfga begcd cbg gc gcadebf fbgde acbgfd abcde gfcbed gfec | fcgedb cgb dgebacf gc",
        "fgaebd cg bdaec gdafb agbcfd gdcbef bgcad gfac gcb cdgabef | cg cg fdcagb cbg",
        "fbegcd cbd adcefb dageb afcb bc aefdc ecdab fgdeca fcdbega | efabcd cedba gadfec cb",
        "aecbfdg fbg gf bafeg dbefa fcge gcbea fcaegb dgceab fcbdga | gecf egdcabf bgf bfgea",
        "fgeab ca afcebg bdacfeg cfaedg gcfdb baec bfadeg bafgc acf | gebdcfa ecba ca fadegcb",
        "dbcfg fgd bdegcaf fgec aegbdf ecdfab fbedc dacgb gdcebf gf | cefg dcbef fcge gbcadfe",
        "bdfegc cbegaf gecbf dfcage bdacg ed bedf ced adcbefg gebcd | ed bcgafe cdgba cbgef",
        "egadfb cdbfeg cegd fecab cgb gbdefca cg fgcdab egfdb bfceg | gbdfcae bgc cg cgb",
        "gcafb gcf dcaebfg ecagb gf abcdeg gaef cafbge fdbac fegbdc | fgae cfgab fg bagce",
    };

    protected override ValueTask<object> HandlePart1(string[] input)
    {
        var cnt = 0;
        foreach (var str in input)
        {
            var parts = str.Split('|');

            var identifiers = parts[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Where(x => IsLen(x.Length)).Select(x => x.Sum(Convert.ToInt32)).ToArray();
            var values = parts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Sum(Convert.ToInt32));

            cnt += values.Count(x => identifiers.Contains(x));
        }
        return ValueTask.FromResult<object>(cnt);
    }
    
    
    private bool IsLen(int len) => len switch
    {
        2 => true,
        3 => true,
        4 => true,
        7 => true,
        _ => false
    };

    
    protected override ValueTask<object> HandlePart2(string[] input)
    {
        var sum = 0;
        
        foreach (var str in input)
        {
            var parts = str.Split('|');

            var zero = ReadOnlySpan<char>.Empty;
            var one = parts[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault(x => x.Length == 2).AsSpan();
            var two = ReadOnlySpan<char>.Empty;
            var three = ReadOnlySpan<char>.Empty;
            var four = parts[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault(x => x.Length == 4).AsSpan();
            var five = ReadOnlySpan<char>.Empty;
            var six = ReadOnlySpan<char>.Empty;
            var seven = parts[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault(x => x.Length == 3).AsSpan();
            var eight = parts[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault(x => x.Length == 7).AsSpan();
            var nine = ReadOnlySpan<char>.Empty;

            
            var sixOrNine = parts[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Where(x => x.Length == 6);
            var fiveTwoThree = parts[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Where(x => x.Length == 5);

            foreach (var entry in sixOrNine)
            {
                if (Intersect(entry, four))
                {
                    nine = entry.AsSpan();
                }
                else if (Intersect(entry, seven))
                {
                    zero = entry.AsSpan();
                }
                else
                {
                    six = entry.AsSpan();
                }
            }

            foreach (var entry in fiveTwoThree)
            {
                if (Intersect(entry, seven))
                {
                    three = entry.AsSpan();
                }
                else if (Intersect(six, entry))
                {
                    five = entry.AsSpan();
                }
                else
                {
                    two = entry.AsSpan();
                }
            }

            var output = string.Empty;

            foreach (var part in parts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries))
            {
                var value = part.AsSpan();
                if (Exact(value, zero)) output += "0";
                else if (Exact(value, one)) output += "1";
                else if (Exact(value, two)) output += "2";
                else if (Exact(value, three)) output += "3";
                else if (Exact(value, four)) output += "4";
                else if (Exact(value, five)) output += "5";
                else if (Exact(value, six)) output += "6";
                else if (Exact(value, seven)) output += "7";
                else if (Exact(value, eight)) output += "8";
                else if (Exact(value, nine)) output += "9";
            }

            var intValue = int.Parse(output);
            sum += intValue;
        }
        
        return ValueTask.FromResult<object>(sum);
    }

    private bool Intersect(ReadOnlySpan<char> target, ReadOnlySpan<char> comparator)
    {
        foreach (var c in comparator)
        {
            if (target.IndexOf(c) == -1) return false;
        }

        return true;
    }
    
    private bool Exact(ReadOnlySpan<char> target, ReadOnlySpan<char> comparator)
    {
        foreach (var c in comparator)
        {
            if (target.IndexOf(c) == -1) return false;
        }

        return target.Length == comparator.Length;
    }
}