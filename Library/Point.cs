namespace AdventOfCode.Library;

public struct Point<T>
{
    public T Value { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
}

public record struct Point(int X, int Y)
{
    public static Point operator +(Point a, Point b) => new Point() { X = a.X + b.X, Y = a.Y + b.Y };

    public double EuclidianDistance(Point a)
    {
        return Math.Sqrt(Math.Pow(a.X - X, 2) + Math.Pow(a.Y - Y, 2));
    }
}