using System.Diagnostics;

namespace AdventOfCode.Library;

public abstract class DayEngine
{
    private readonly InputFetcher inputFetcher = new InputFetcher();

    public int Year => int.Parse(this.GetType().Namespace!.Split('.').Last().Replace("_", ""));
    public int Day => int.Parse(this.GetType().Name[3..]);

    public abstract string[] TestInput { get; }
    private string[] input = Array.Empty<string>();

    protected abstract object HandlePart1(string[] input);
    protected abstract object HandlePart2(string[] input);

    protected virtual string[] Transform(string str) => str.Split("\n", StringSplitOptions.RemoveEmptyEntries);

    public async ValueTask Setup() => input = Transform(await inputFetcher.FetchInput(Year, Day));
    
    public void Run()
    {
        var result1 = HandlePart1(input);
        PrintResult(1, result1);

        var result2 = HandlePart2(input);
        PrintResult(2, result2);
    }

    public void RunTests()
    {
        var result1 = HandlePart1(TestInput);
        PrintResult(1, result1);

        var result2 = HandlePart2(TestInput);
        PrintResult(2, result2);
    }

    public double RunTimed()
    {
        const int Iterations = 100;
        const double DIterations = 100d;

        object? result1 = default;
        object? result2 = default;
        
        
        for (int i = 0; i < 10; i++)
        {
            var _ = HandlePart1(input);
        }
        
        var start = Stopwatch.GetTimestamp();
        
        for (int i = 0; i < Iterations; i++)
        {
            result1 = HandlePart1(input);
        }

        var stop = Stopwatch.GetTimestamp();

        var avgTime1 = (stop - start) / DIterations;
        var part1Time = Utils.GetTimeString(avgTime1);
        
        for (int i = 0; i < 10; i++)
        {
            var _ = HandlePart2(input);
        }
        
        start = Stopwatch.GetTimestamp();
        
        for (int i = 0; i < Iterations; i++)
        {
            result2 = HandlePart2(input);
        }

        stop = Stopwatch.GetTimestamp();
        
        var avgTime2 = (stop - start) / DIterations;
        var part2Time = Utils.GetTimeString(avgTime2);
        
        PrintPerformanceResult(1, part1Time, result1);
        PrintPerformanceResult(2, part2Time, result2);

        return avgTime1 + avgTime2;
    }
    
    private void PrintResult(int part, object? result) => Console.WriteLine($"Day: {Day}, Part: {part} -- Result is: {result?.ToString()}");
    private void PrintPerformanceResult(int part, string elapsed, object? result) => Console.WriteLine($"Day: {Day, 1} - Part: {part, 1} - Elapsed: {elapsed, 12} -- Result is: {result?.ToString(), 16}");
}