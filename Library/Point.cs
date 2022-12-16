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
    public static Point operator -(Point a, Point b) => new Point() { X = a.X - b.X, Y = a.Y - b.Y };
    public static Point operator *(Point a, Point b) => new Point() { X = a.X * b.X, Y = a.Y * b.Y };
    public static Point operator *(Point a, int b) => new Point() { X = a.X * b, Y = a.Y * b };

    public double EuclidianDistance(Point a)
    {
        return Math.Sqrt(Math.Pow(a.X - X, 2) + Math.Pow(a.Y - Y, 2));
    }
    
    public int ManhattanDistance(Point a)
    {
        return Math.Abs(a.X - X) + Math.Abs(a.Y - Y);
    }

    public int Magnitude() => Math.Abs(X) + Math.Abs(Y);
    public Point Normalize() => new Point(Math.Clamp(Math.Abs(X), 0, 1) * Math.Sign(X), Math.Clamp(Math.Abs(Y), 0, 1) * Math.Sign(Y));
    
    public Point<T> Value<T>(T value) => new Point<T>(X, Y) { Value = value };
}