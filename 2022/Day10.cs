using AdventOfCode.Library;

namespace AdventOfCode._2022;

public class Day10 : DayEngine
{
    public override string[] TestInput => new string[]
    {
        "addx 15",
        "addx -11",
        "addx 6",
        "addx -3",
        "addx 5",
        "addx -1",
        "addx -8",
        "addx 13",
        "addx 4",
        "noop",
        "addx -1",
        "addx 5",
        "addx -1",
        "addx 5",
        "addx -1",
        "addx 5",
        "addx -1",
        "addx 5",
        "addx -1",
        "addx -35",
        "addx 1",
        "addx 24",
        "addx -19",
        "addx 1",
        "addx 16",
        "addx -11",
        "noop",
        "noop",
        "addx 21",
        "addx -15",
        "noop",
        "noop",
        "addx -3",
        "addx 9",
        "addx 1",
        "addx -3",
        "addx 8",
        "addx 1",
        "addx 5",
        "noop",
        "noop",
        "noop",
        "noop",
        "noop",
        "addx -36",
        "noop",
        "addx 1",
        "addx 7",
        "noop",
        "noop",
        "noop",
        "addx 2",
        "addx 6",
        "noop",
        "noop",
        "noop",
        "noop",
        "noop",
        "addx 1",
        "noop",
        "noop",
        "addx 7",
        "addx 1",
        "noop",
        "addx -13",
        "addx 13",
        "addx 7",
        "noop",
        "addx 1",
        "addx -33",
        "noop",
        "noop",
        "noop",
        "addx 2",
        "noop",
        "noop",
        "noop",
        "addx 8",
        "noop",
        "addx -1",
        "addx 2",
        "addx 1",
        "noop",
        "addx 17",
        "addx -9",
        "addx 1",
        "addx 1",
        "addx -3",
        "addx 11",
        "noop",
        "noop",
        "addx 1",
        "noop",
        "addx 1",
        "noop",
        "noop",
        "addx -13",
        "addx -19",
        "addx 1",
        "addx 3",
        "addx 26",
        "addx -30",
        "addx 12",
        "addx -1",
        "addx 3",
        "addx 1",
        "noop",
        "noop",
        "noop",
        "addx -9",
        "addx 18",
        "addx 1",
        "addx 2",
        "noop",
        "noop",
        "addx 9",
        "noop",
        "noop",
        "noop",
        "addx -1",
        "addx 2",
        "addx -37",
        "addx 1",
        "addx 3",
        "noop",
        "addx 15",
        "addx -21",
        "addx 22",
        "addx -6",
        "addx 1",
        "noop",
        "addx 2",
        "addx 1",
        "noop",
        "addx -10",
        "noop",
        "noop",
        "addx 20",
        "addx 1",
        "addx 2",
        "addx 2",
        "addx -6",
        "addx -11",
        "noop",
        "noop",
        "noop",
    };

    protected override object HandlePart1(string[] input)
    {
        var instructions = input.Select(Parse).ToList();
        var currentInstructionIndex = 0;
        var registerX = 1;
        var complete = false;

        var indexes = new[] { 20, 60, 100, 140, 180, 220 };
        var max = indexes.Last();

        var returnVal = 0;
        
        for (int i = 1; i <= max; i++)
        {
            if (i == indexes.First())
            {
                returnVal += indexes.First() * registerX;
                indexes = indexes.Skip(1).ToArray();
            }
            
            (complete, registerX) = instructions[currentInstructionIndex].Execute(registerX);
            if (complete) currentInstructionIndex += 1;
        }
        
        return returnVal;
    }

    protected override object HandlePart2(string[] input)
    {
        var instructions = input.Select(Parse).ToList();
        var currentInstructionIndex = 0;
        var registerX = 1;
        var complete = false;

        var crt = Grid<char>.Create(40, 6);

        for (var i = 0;currentInstructionIndex < instructions.Count;i++)
        {
            var translatedI = i % 40; 
            var col = i / 40;
            if (Math.Abs(translatedI - registerX) < 2)
            {
                crt[col, translatedI] = '#';
            }
            else
            {
                crt[col, translatedI] = '.';
            }
            
            (complete, registerX) = instructions[currentInstructionIndex].Execute(registerX);
            if (complete) currentInstructionIndex += 1;
        }

        var lines = new string[crt.Size+1];
        lines[0] = "";
        for (int i = 1; i < crt.Size + 1; i++)
        {
            lines[i] = new string(crt.GetRow(i - 1));
        }
        
        return string.Join(Environment.NewLine, lines);
    }




    private Instruction Parse(string cmd)
    {
        var parts = cmd.Split(' ');
        return parts[0] switch
        {
            "noop" => new Noop(),
            "addx" => new AddX(parts[1].Int())
        };
    }
    
    private abstract record Instruction(int ClockCycles)
    {
        private int remainingCycles = ClockCycles;

        public (bool, int) Execute(int registerX)
        {
            remainingCycles += -1;
            var isDone = remainingCycles <= 0;
            if (isDone) return (isDone, F(registerX));
            return (isDone, registerX);
        }

        protected abstract int F(int r);
    }

    private record Noop() : Instruction(1)
    {
        protected override int F(int r)
        {
            return r;
        }
    }

    private record AddX(int Value) : Instruction(2)
    {
        protected override int F(int r)
        {
            return r + Value;
        }
    }
}

