namespace AdventOfCode.Library;

public class Grid<T>
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
    
    private T? GetElementOrDefault(int x, int y) => IsYValueWithinBounds(y) && IsXValueWithinBounds(x) ? grid[y][x] : default;
    public T? this[int y, int x] => GetElementOrDefault(x, y);
    public T[][] GetRaw() => grid;

    public T[] GetRow(int y) => grid[y];
    public T[] GetColumn(int x) => columns[x];
}