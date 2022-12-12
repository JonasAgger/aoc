namespace AdventOfCode.Library;

public record struct Point<T>(int X, int Y)
{
    public T Value { get; set; }
    
    public double EuclidianDistance(Point a)
    {
        return Math.Sqrt(Math.Pow(a.X - X, 2) + Math.Pow(a.Y - Y, 2));
    }

    public Point AsPoint() => new Point(X, Y);
}

public record struct Point(int X, int Y) 
{
    public static Point None => new Point(-1, -1);
    
    public static Point operator +(Point a, Point b) => new Point() { X = a.X + b.X, Y = a.Y + b.Y };

    public double EuclidianDistance(Point a)
    {
        return Math.Sqrt(Math.Pow(a.X - X, 2) + Math.Pow(a.Y - Y, 2));
    }

    public Point<T> Value<T>(T value) => new Point<T>(X, Y) { Value = value };
}