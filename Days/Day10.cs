using System;
using AdventOfCode.Library;

namespace AdventOfCode.Days;

public class Day10 : DayEngine
{
    public override string[] TestInput => new string[]
    {
        "[({(<(())[]>[[{[]{<()<>>",
        "[(()[<>])]({[<{<<[]>>(",
        "{([(<{}[<>[]}>{[]{[(<()>",
        "(((({<>}<{<{<>}{[]{[]{}",
        "[[<[([]))<([[{}[[()]]]",
        "[{[{({}]{}}([{[{{{}}([]",
        "{<[[]]>}<{[{[{[]{()[[[]",
        "[<(<(<(<{}))><([]([]()",
        "<{([([[(<>()){}]>(<<{{",
        "<{([{{}}[<[[[<>{}]]]>[]]",
    };

    private const char Start1 = '(';
    private const char Start2 = '[';
    private const char Start3 = '{';
    private const char Start4 = '<';
    
    private const char End1 = ')';
    private const char End2 = ']';
    private const char End3 = '}';
    private const char End4 = '>';

    private char[] StartingChunks = new[]
    {
        Start1,
        Start2,
        Start3,
        Start4,
    };

    private char[] EndingChunks = new[]
    {
        End1,
        End2,
        End3,
        End4,
    };
    
    protected override Task<object> HandlePart1(string[] input)
    {
        var syntaxErrorScore = 0;
        
        foreach (var line in input)
        {
            var (corrupted, found, expected, _) = IsCorrupt(line, new Deque<Scope>());

            if (corrupted)
            {
                syntaxErrorScore += ConvertToPoints(found!.Value);
                //Console.WriteLine($"{line} ----- {corrupted} ----- {found} - {expected}");
            }
        }
        
        return Task.FromResult<object>(syntaxErrorScore);
    }

    protected override Task<object> HandlePart2(string[] input)
    {
        var syntaxCompleteScores = new List<long>();
        
        foreach (var line in input)
        {
            var (corrupted, _, _, deque) = IsCorrupt(line, new Deque<Scope>());

            if (corrupted) continue;
            
            var scope = deque.Pop();
            long syntaxCompleteScore = 0;
            while (scope is { })
            {
                syntaxCompleteScore *= 5;
                    
                var closingChar = GetOpposite(scope.StartingChar);
                syntaxCompleteScore += ConvertToPoints2(closingChar);
                    
                scope = deque.Pop();
            }
            syntaxCompleteScores.Add(syntaxCompleteScore);
        }

        
        return Task.FromResult<object>(syntaxCompleteScores.OrderBy(x => x).Skip(syntaxCompleteScores.Count/2).First());
    }

    private char GetOpposite(char entry) => entry switch
    {
        Start1 => End1,
        Start2 => End2,
        Start3 => End3,
        Start4 => End4,
        End1 => Start1,
        End2 => Start2,
        End3 => Start3,
        End4 => Start4,
        _ => '?'
    };

    private bool IsEndingChar(char entry, char opening) => (entry, opening) switch
    {
        (End1, Start1) => true,
        (End2, Start2) => true,
        (End3, Start3) => true,
        (End4, Start4) => true,
        _ => false
    };
    
    private int ConvertToPoints(char entry) => entry switch
    {
        End1 => 3, 
        End2 => 57,
        End3 => 1197,
        End4 => 25137,
        _ => 0 
    };
    
    private int ConvertToPoints2(char entry) => entry switch
    {
        End1 => 1, 
        End2 => 2,
        End3 => 3,
        End4 => 4,
        _ => 0 
    };
    
    private (bool, char? found, char? expected, Deque<Scope> remainingQueue) IsCorrupt(ReadOnlySpan<char> line, Deque<Scope> scopeQueue)
    {
        foreach (var entry in line)
        {
            if (StartingChunks.Contains(entry))
            {
                scopeQueue.Add(new Scope(entry));
            }
            if (EndingChunks.Contains(entry))
            {
                var currentScope = scopeQueue.Pop();
                if (!IsEndingChar(entry, currentScope?.StartingChar ?? char.MaxValue))
                    return (true, entry, GetOpposite(currentScope!.StartingChar), scopeQueue);
            }
        }

        return (false, null, null, scopeQueue);
    }
    
    private record Scope(char StartingChar);
}