namespace AdventOfCode.Library;

public abstract class DayEngine
{
    private readonly InputFetcher inputFetcher = new InputFetcher();

    public int Year => int.Parse(this.GetType().Namespace!.Split('.').Last().Replace("_", ""));
    public int Day => int.Parse(this.GetType().Name[3..]);

    public abstract string[] TestInput { get; }

    protected abstract ValueTask<object> HandlePart1(string[] input);
    protected abstract ValueTask<object> HandlePart2(string[] input);

    protected virtual string[] Transform(string str) => str.Split("\n", StringSplitOptions.RemoveEmptyEntries);

    public async ValueTask Run()
    {
        var input = await inputFetcher.FetchInput(Year, Day);

        var transformedInput = Transform(input);

        var result1 = await HandlePart1(transformedInput);
        var result2 = await HandlePart2(transformedInput);

        PrintResult(1, result1);
        PrintResult(2, result2);
    }

    public async ValueTask RunTests()
    {
        var result1 = await HandlePart1(TestInput);
        var result2 = await HandlePart2(TestInput);

        PrintResult(1, result1);
        PrintResult(2, result2);
    }

    protected void PrintResult(int part, object result) => Console.WriteLine($"Day: {Day}, Part: {part} -- Result is: {result?.ToString()}");
}