using AdventOfCode.Library;

namespace AdventOfCode._2022;

public class Day2 : DayEngine
{
    private enum Score
    {
        Win = 6,
        Lose = 0,
        Draw = 3,
    }

    private Dictionary<string, string> Beats = new()
    {
        ["A"] = "C",
        ["B"] = "A",
        ["C"] = "B",
    };
    
    private Dictionary<string, string> Alias = new()
    {
        ["X"] = "A",
        ["Y"] = "B",
        ["Z"] = "C",
    };
    
    private Dictionary<string, Score> NeededResult = new()
    {
        ["X"] = Score.Lose,
        ["Y"] = Score.Draw,
        ["Z"] = Score.Win,
    };
    
    private Dictionary<string, int> Scores = new()
    {
        ["A"] = 1,
        ["B"] = 2,
        ["C"] = 3,
    };

    public override string[] TestInput => new[]
    {
        "A Y",
        "B X",
        "C Z",
    };
    
    protected override object HandlePart1(string[] input)
    {
        var score = 0;

        foreach (var round in input)
        {
            var opponent = round.Split()[0];
            var me = Alias[round.Split()[1]];

            Score result;
            if (me == opponent) result = Score.Draw;
            else if (Beats[me] == opponent) result = Score.Win;
            else result = Score.Lose;

            score += (int)result;
            score += Scores[me];
        }
        
        
        return score;
    }

    protected override object HandlePart2(string[] input)
    {
        var score = 0;

        foreach (var round in input)
        {
            var opponent = round.Split()[0];
            var neededResult = NeededResult[round.Split()[1]];

            var me = neededResult switch
            {
                Score.Win => Beats.First(x => x.Value == opponent).Key,
                Score.Lose => Beats[opponent],
                Score.Draw => opponent,
            };
            
            
            Score result;
            if (me == opponent) result = Score.Draw;
            else if (Beats[me] == opponent) result = Score.Win;
            else result = Score.Lose;

            score += (int)result;
            score += Scores[me];
        }
        
        
        return score;
    }
}

