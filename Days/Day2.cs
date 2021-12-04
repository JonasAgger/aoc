using AdventOfCode.Library;
using AdventOfCode.Sub;

namespace AdventOfCode.Days;

public class Day2 : DayEngine
{
    private Submarine1 sub = new Submarine1();
    private Submarine2 sub2 = new Submarine2();

    public override string[] TestInput => new string[]
    {
        "forward 5",
        "down 5",
        "forward 8",
        "up 3",
        "down 8",
        "forward 2",
    };

    protected override Task<object> HandlePart1(string[] input)
    {
        foreach (var (direction, magnitude) in input.Select(x => x.FromString()))
        {
            sub.Move(direction, magnitude);
        }


        return Task.FromResult<object>(sub.HorizontalPosition * sub.Depth);
    }

    protected override Task<object> HandlePart2(string[] input)
    {
        foreach (var (direction, magnitude) in input.Select(x => x.FromString()))
        {
            sub2.Move(direction, magnitude);
        }


        return Task.FromResult<object>(sub2.HorizontalPosition * sub2.Depth);
    }

    public class Submarine1
    {
        public int HorizontalPosition { get; private set; }
        public int Depth { get; private set; }

        public void Move(Direction direction, int magnitude)
        {
            switch (direction)
            {
                case Direction.Up:
                    Depth -= magnitude;
                    break;

                case Direction.Down:
                    Depth += magnitude;
                    break;

                case Direction.Forward:
                    HorizontalPosition += magnitude;
                    break;
                case Direction.Backward:
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }
    }

    public class Submarine2
    {
        public int HorizontalPosition { get; private set; }
        public int Depth { get; private set; }
        public int Aim { get; private set; }

        public void Move(Direction direction, int magnitude)
        {
            switch (direction)
            {
                case Direction.Up:
                    Aim -= magnitude;
                    break;

                case Direction.Down:
                    Aim += magnitude;
                    break;

                case Direction.Forward:
                    HorizontalPosition += magnitude;
                    Depth += magnitude * Aim;
                    break;
                case Direction.Backward:
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }
    }
}