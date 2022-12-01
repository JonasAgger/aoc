namespace AdventOfCode.Sub;

public static class SubmarineHelper
{
    public static (Direction, int) FromString(this string str)
    {
        var split = str.Split();

        return (Enum.Parse<Direction>(split[0], true), int.Parse(split[1]));
    }
}