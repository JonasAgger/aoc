namespace AdventOfCode.Bingo;

public class BingoBoard
{
    private readonly BingoBoardEntry[][] bingoBoard;

    private bool isWinner = false;

    private int numbersSeen = 0;
    
    public BingoBoard(string[] strs)
    {
        bingoBoard = new BingoBoardEntry[strs.Length][];

        for (int i = 0; i < strs.Length; i++)
        {
            var entries = strs[i].Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            bingoBoard[i] = entries.Select(int.Parse).Select(x => new BingoBoardEntry(){Number = x}).ToArray();
        }
    }

    public int UnCalledNumbersSummed() => bingoBoard.SelectMany(x => x).Where(x => !x.HasBeenCalled).Sum(x => x.Number);
    
    public void CallNumber(int number)
    {
        CheckNumber(number);

        if (numbersSeen >= 5)
        {
            CheckForWinning();
        }
    }

    private void CheckNumber(int number)
    {
        foreach (var row  in bingoBoard)
        {
            foreach (var column in row)
            {
                if (column.Number == number)
                {
                    column.HasBeenCalled = true;
                    numbersSeen += 1;
                    return;
                }
            }
        }
    }

    private void CheckForWinning()
    {
        if (isWinner) return;
        var columnsToCheck = Enumerable.Range(0,5).Select(x => true).ToArray();

        for (int i = 0; i < bingoBoard.Length; i++)
        {
            isWinner = CheckRow(i, columnsToCheck);
            if (isWinner) return;
        }

        isWinner = columnsToCheck.Any(x => x == true);
    }

    public bool IsWinner()
    {
        return isWinner;
    }

    private bool CheckRow(int row, bool[] columnsToCheck)
    {
        var isWinningRow = true;
        for (int i = 0; i < bingoBoard.Length; i++)
        {
            columnsToCheck[i] = columnsToCheck[i] && bingoBoard[row][i].HasBeenCalled;
            isWinningRow = isWinningRow && bingoBoard[row][i].HasBeenCalled;
        }

        return isWinningRow;
    }
    
    private class BingoBoardEntry
    {
        public int Number { get; set; }
        public bool HasBeenCalled { get; set; }
    }
}