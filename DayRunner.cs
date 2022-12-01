using AdventOfCode.Days;
using AdventOfCode.Library;

namespace AdventOfCode;

public class DayRunner
{
    public async ValueTask Run(string? year = null)
    {
        var days = typeof(DayEngine)
            .Assembly
            .GetTypes()
            .Where(x => x.IsAssignableTo(typeof(DayEngine)) && x.Name.StartsWith("Day") && x.IsAbstract == false && x.Name != nameof(Day0) && (year == null ? !x.Namespace!.Contains("Old") : x.Namespace!.Contains(year)))
            .OrderBy(x => int.Parse(x.Name[3..]))
            .ToArray();

        Console.WriteLine("Select Day:");

        var index = 1;
        
        foreach (var day in days)
        {
            Console.WriteLine($"{index++}: {day.Name}");
        }

        index = int.Parse(Console.ReadLine() ?? "-1");

        if (index > days.Length || index < 1)
            return;
        
        Console.WriteLine("Is Test? y/n");

        var isTest = Console.ReadKey().Key == ConsoleKey.Y;

        Console.WriteLine();
        
        var selectedDay = (DayEngine)Activator.CreateInstance(days[index-1])!;

        Console.WriteLine($"--- Running {selectedDay.GetType().Name} as {(isTest ? "Test" : "Actual")} ---");
        
        if (isTest)
        {
            selectedDay.RunTests();
        }
        else
        {
            await selectedDay.Setup();
            selectedDay.Run();
        }
    }

    public async ValueTask RunTimed(string? year = null, int? specificDay = null)
    {
        var days = typeof(DayEngine)
            .Assembly
            .GetTypes()
            .Where(x => x.IsAssignableTo(typeof(DayEngine)) && x.Name.StartsWith("Day") && x.IsAbstract == false && x != typeof(Day0) && (year == null ? !x.Namespace!.Contains("Old") : x.Namespace!.Contains(year)))
            .OrderBy(x => int.Parse(x.Name[3..]))
            .Select(x => (DayEngine)Activator.CreateInstance(x)!)
            .ToArray();

        if (specificDay != null)
        {
            days = days.Where(x => x.Day == specificDay.Value).ToArray();
        }
        
        var totalTime = 0d;

        foreach (var day in days)
            await day.Setup();
        
        foreach (var day in days)
            totalTime += day.RunTimed();
        
        Console.WriteLine("---------------------------------------------");
        Console.WriteLine($"Total Time: {Utils.GetTimeString(totalTime)}");
    }
}