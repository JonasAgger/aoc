using System.Reflection;
using AdventOfCode._2021;
using AdventOfCode.Library;

namespace AdventOfCode;

public class DayRunner
{
    public async Task Run()
    {
        // only 2022
        var days = typeof(DayEngine)
            .Assembly
            .GetTypes()
            .Where(x => x.IsAssignableTo(typeof(DayEngine)) && x.Name.StartsWith("Day") && x.IsAbstract == false && x.Name != nameof(Day0))
            .Where(x => x.Namespace!.Contains("2022"))
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
            await selectedDay.RunTests();
        }
        else
        {
            await selectedDay.Run();
        }
    }
}