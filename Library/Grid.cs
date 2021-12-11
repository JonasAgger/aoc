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

    private Grid(T[][] grid) => this.grid = grid;

    private T[][] grid;
    private bool IsWithInBounds(int val, int len) => val >= 0 && val < len;

    public int Size => grid.Length;
    public int RowSize(int index = 0) => grid[index].Length;

    public bool IsYValueWithinBounds(int y) => IsWithInBounds(y, grid.Length);
    public bool IsXValueWithinBounds(int x) => IsWithInBounds(x, grid[0].Length);
    
    private T? GetElementOrDefault(int x, int y) => IsYValueWithinBounds(y) && IsXValueWithinBounds(x) ? grid[y][x] : default;
    public T? this[int y, int x] => GetElementOrDefault(x, y);
}