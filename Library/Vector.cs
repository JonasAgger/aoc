namespace AdventOfCode.Library;

public record struct Vector(int XMagnitude, int YMagnitude)
{
    public Vector Inverse() => new Vector(YMagnitude, XMagnitude);
    public Vector Abs() => new Vector(Math.Abs(XMagnitude), Math.Abs(YMagnitude));
    public Vector TurnRight()
    {
        return new Vector(-YMagnitude, XMagnitude);
    }
    
    public Vector TurnLeft()
    {
        return new Vector(YMagnitude, -XMagnitude);
    }

    public static List<Vector> Generate8DirectionalForGrid<T>(int length, Point p, Grid<T> grid) where T: IEquatable<T>
    {
        var vectors = GenerateStraightDirectionalForGrid(length, p, grid);
        var diagonal = GenerateDiagonalDirectionalForGrid(length, p, grid);

        foreach (var vec in diagonal)
        {
            vectors.Add(vec);
        }
        
        return vectors;
    }
    
    public static List<Vector> GenerateDiagonalDirectionalForGrid<T>(int length, Point p, Grid<T> grid) where T: IEquatable<T>
    {
        var vectors = new List<Vector>();
        
        // LeftDown, RightDown, LeftUp, RightUp
        if (grid.IsPointWithinBounds(p + new Point(-length, -length)))
        {
            vectors.Add(new Vector(-1, -1));
        }
        if (grid.IsPointWithinBounds(p + new Point(length, -length)))
        {
            vectors.Add(new Vector(1, -1));
        }
        if (grid.IsPointWithinBounds(p + new Point(-length, length)))
        {
            vectors.Add(new Vector(-1, 1));
        }
        if (grid.IsPointWithinBounds(p + new Point(length, length)))
        {
            vectors.Add(new Vector(1, 1));
        }

        return vectors;
    }
    
    public static List<Vector> GenerateStraightDirectionalForGrid<T>(int length, Point p, Grid<T> grid) where T: IEquatable<T>
    {
        var vectors = new List<Vector>();
        // Down, Up, Left, Right
        if (grid.IsPointWithinBounds(p + new Point(0, -length)))
        {
            vectors.Add(new Vector(0, -1));
        }
        if (grid.IsPointWithinBounds(p + new Point(0, length)))
        {
            vectors.Add(new Vector(0, 1));
        }
        if (grid.IsPointWithinBounds(p + new Point(-length, 0)))
        {
            vectors.Add(new Vector(-1, 0));
        }
        if (grid.IsPointWithinBounds(p + new Point(length, 0)))
        {
            vectors.Add(new Vector(1, 0));
        }
        
        return vectors;
    }
    
    public static List<Vector> GenerateStraightDirectionalForGridUnchecked<T>(int length, Point p, Grid<T> grid) where T: IEquatable<T>
    {
        var vectors = new List<Vector>();
        // Down, Up, Left, Right
        vectors.Add(new Vector(0, -1));
        vectors.Add(new Vector(0, 1));
        vectors.Add(new Vector(-1, 0));
        vectors.Add(new Vector(1, 0));
        
        return vectors;
    }
    
    public static Point operator +(Point a, Vector b) => new Point() { X = a.X + b.XMagnitude, Y = a.Y + b.YMagnitude };
    public static Point operator *(Point a, Vector b) => new Point() { X = a.X * b.XMagnitude, Y = a.Y * b.YMagnitude };
    public static Point operator -(Point a, Vector b) => new Point() { X = a.X - b.XMagnitude, Y = a.Y - b.YMagnitude };

    public static Vector operator *(Vector a, int b) => new Vector(a.XMagnitude * b, a.YMagnitude * b);

    public override string ToString()
    {
        return $"Vec: {XMagnitude}, {YMagnitude}";
    }
}