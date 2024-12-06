namespace AdventOfCode.Library;

public class Grid<T> where T : IEquatable<T>
{
    public static Grid<T> Create(string[] input, Func<string, T[]> transformer)
    {
        return new Grid<T>(input.Select(transformer).ToArray());
    }
    
    public static Grid<T> Create(string[] input, Func<char, T> transformer)
    {
        return new Grid<T>(input.Select(line => line.Select(transformer).ToArray()).ToArray());
    }

    public static Grid<T> Create(string[] input, Func<string, Point<T>> transformer)
    {
        var components = input.Select(x => transformer(x)).ToArray();

        var maxX = components.Max(x => x.X) + 1;
        var maxY = components.Max(x => x.Y) + 1;

        var grid = Enumerable.Range(0, maxY).Select(_ => new T[maxX]).ToArray();

        foreach (var component in components)
            grid[component.Y][component.X] = component.Value;

        return new Grid<T>(grid);
    }

    public static Grid<T> Create(int rows, int columns)
    {
        T[][] grid = new T[columns][];
        for (int i = 0; i < columns; i++)
        {
            grid[i] = new T[rows];
        }

        return new Grid<T>(grid);
    }

    private Grid(T[][] grid)
    {
        this.grid = grid;
        this.columns = Enumerable.Range(0, grid.Length).Select(x => grid.Select(row => row[x]).ToArray()).ToArray();
    }

    private readonly T[][] grid;
    private readonly T[][] columns;
    private bool IsWithInBounds(int val, int len) => val >= 0 && val < len;

    public int Size => grid.Length;
    public int RowSize(int index = 0) => grid[index].Length;

    public bool IsYValueWithinBounds(int y) => IsWithInBounds(y, grid.Length);
    public bool IsXValueWithinBounds(int x) => IsWithInBounds(x, grid[0].Length);
    public bool IsPointWithinBounds(Point p) => IsYValueWithinBounds(p.Y) && IsXValueWithinBounds(p.X);
    
    private T? GetElementOrDefault(int x, int y) => IsYValueWithinBounds(y) && IsXValueWithinBounds(x) ? grid[y][x] : default;
    private T? GetElementOrDefault(Point p) => GetElementOrDefault(p.X, p.Y);

    public Point IndexOf(T value)
    {
        for (int i = 0; i < grid.Length; i++)
        {
            var index = grid[i].AsSpan().IndexOf(value);
            if (index != -1) return new Point(index, i);
        }

        return Point.None;
    }
    
    public List<Point> IndexOfAll(T value)
    {
        var points = new List<Point>();
        for (int i = 0; i < grid.Length; i++)
        {
            var index = grid[i].AsSpan().IndexOf(value);
            while (index != -1)
            {
                points.Add(new Point(index, i));
                var nextIndex = grid[i].AsSpan(index + 1).IndexOf(value);
                if (nextIndex == -1) break;
                index = nextIndex + index + 1;
            }
        }

        return points;
    }
    
    public T? this[int y, int x]
    {
        get => GetElementOrDefault(x, y);
        set => grid[y][x] = value;
    }
    
    public T? this[Point p]
    {
        get => GetElementOrDefault(p.X, p.Y);
        set => grid[p.Y][p.X] = value;
    }
    public T[][] GetRaw() => grid;

    public T[] GetRow(int y) => grid[y];
    public T[] GetColumn(int x) => columns[x];
}