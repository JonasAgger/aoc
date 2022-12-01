using AdventOfCode.Bingo;
using AdventOfCode.Library;

namespace AdventOfCode._2021;

public class Day4 : DayEngine
{
    public override string[] TestInput => new string[]
    {
        "7,4,9,5,11,17,23,2,0,14,21,24,10,16,13,6,15,25,12,22,18,20,8,19,3,26,1",

        "22 13 17 11  0",
        "8  2 23  4 24",
        "21  9 14 16  7",
        "6 10  3 18  5",
        "1 12 20 15 19",

        "3 15 0 2 22",
        "9 18 13 17  5",
        "19  8  7 25 23",
        "20 11 10 24  4",
        "14 21 16 12  6",

        "14 21 17 24  4",
        "10 16 15  9 19",
        "18  8 23 26 20",
        "22 11 13  6  5",
        "2  0 12  3  7",
    };

    private List<int> bingoNumbers = new();

    private List<BingoBoard> bingoBoards = new();

    protected override ValueTask<object> HandlePart1(string[] input)
    {
        bingoNumbers = input[0].Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();

        bingoBoards = input.Skip(1).Chunk(5).Select(x => new BingoBoard(x)).ToList();

        foreach (var bingoNumber in bingoNumbers)
        {
            Console.WriteLine(bingoNumber);
            
            foreach (var bingoBoard in bingoBoards)
            {
                bingoBoard.CallNumber(bingoNumber);

                if (bingoBoard.IsWinner())
                {
                    var unCalledSum = bingoBoard.UnCalledNumbersSummed();
                    
                    return ValueTask.FromResult<object>(unCalledSum * bingoNumber);
                }
            }
        }
        
        return ValueTask.FromResult<object>(null);
    }

    protected override ValueTask<object> HandlePart2(string[] input)
    {
        bingoNumbers = input[0].Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();

        bingoBoards = input.Skip(1).Chunk(5).Select(x => new BingoBoard(x)).ToList();

        var winningBoards = new List<BingoBoard>();
        
        foreach (var bingoNumber in bingoNumbers)
        {
            Console.WriteLine(bingoNumber);
            
            foreach (var bingoBoard in bingoBoards.Where(x => !x.IsWinner()))
            {
                bingoBoard.CallNumber(bingoNumber);

                if (bingoBoard.IsWinner())
                {
                    winningBoards.Add(bingoBoard);

                    if (winningBoards.Count == bingoBoards.Count)
                    {
                        var unCalledSum = bingoBoard.UnCalledNumbersSummed();
                    
                        return ValueTask.FromResult<object>(unCalledSum * bingoNumber);
                    }
                }
            }
        }
        
        return ValueTask.FromResult<object>(null);
    }
}