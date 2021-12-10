using AdventOfCode.Library;

namespace AdventOfCode.Days;

public class Day8 : DayEngine
{
    public override string[] TestInput => new string[]
    {
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

    protected override Task<object> HandlePart1(string[] input)
    {
        var cnt = 0;
        foreach (var str in input)
        {
            var parts = str.Split('|');

            var identifiers = parts[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Where(x => IsLen(x.Length)).Select(x => x.Sum(Convert.ToInt32)).ToArray();
            var values = parts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Sum(Convert.ToInt32));

            cnt += values.Count(x => identifiers.Contains(x));
        }


        return Task.FromResult<object>(cnt);
    }

    protected override Task<object> HandlePart2(string[] input)
    {

        return Task.FromResult<object>(null);
    }

    private bool IsLen(int len) => len switch
    {
        2 => true,
        3 => true,
        4 => true,
        7 => true,
        _ => false
    };
}