namespace AdventOfCode.Sub;

public class Submarine
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