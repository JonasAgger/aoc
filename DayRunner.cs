using System.Reflection;
using AdventOfCode.Library;

namespace AdventOfCode;

public class DayRunner
{
    public async Task GenerateMissing(int year, string baseLoc)
    {
        const string Template = @"using AdventOfCode.Library;

    namespace AdventOfCode._$YEAR$;

    public class Day$DAY$ : DayEngine
    {
        public override string[] TestInput => new string[]
        {
            """"
        };

        protected override object HandlePart1(string[] input)
        {

            return null;
        }

        protected override object HandlePart2(string[] input)
        {

            return null;
        }
    }";
        
        var yearPath = Path.Combine(baseLoc, year.ToString());

        for (int day = 1; day <= 25; day++)
        {
            var file = Path.Combine(yearPath, $"Day{day:00}.cs");
            if (!File.Exists(file))
            {
                await File.WriteAllTextAsync(file, Template.Replace("$DAY$", day.ToString()).Replace("$YEAR$", year.ToString()));
            }
        }  
    }
    
    public async ValueTask Run(int? year = null)
    {
        var days = typeof(DayEngine)
            .Assembly
            .GetTypes()
            .Where(x => x.IsAssignableTo(typeof(DayEngine)) && x.Name.StartsWith("Day") && x.IsAbstract == false && (year == null ? !x.Namespace!.Contains("Old") : x.Namespace!.Contains(year.ToString())))
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
            if (selectedDay.TestInput.All(string.IsNullOrEmpty))
            {
                var fetcher = new InputFetcher();
                var str = await fetcher.FetchTestInput(selectedDay.Year, selectedDay.Day);
                var startBlock = "<pre><code>";
                var start = str.AsSpan().IndexOf(startBlock) + startBlock.Length;
                var end = str.AsSpan().IndexOf("</code></pre>");

                var str2 = str.Substring(start, end - start).TrimEnd();

                Console.WriteLine("Insert new TestInput:");
                var template = @$"
public override string[] TestInput => new string[]
{{
        {string.Join($"{Environment.NewLine}\t", str2.Split('\n', StringSplitOptions.RemoveEmptyEntries).Select(x => $"\"{x}\","))}
}};";
                Console.WriteLine(template);
                return;
            }
            selectedDay.RunTests();
        }
        else
        {
            await selectedDay.Setup();
            selectedDay.Run();
        }
    }

    public async ValueTask RunTimed(int? year = null, int? specificDay = null)
    {
        var days = typeof(DayEngine)
            .Assembly
            .GetTypes()
            .Where(x => x.IsAssignableTo(typeof(DayEngine)) && x.Name.StartsWith("Day") && x.IsAbstract == false && (year == null ? !x.Namespace!.Contains("Old") : x.Namespace!.Contains(year.ToString())))
            .OrderBy(x => int.Parse(x.Name[3..]))
            .Select(x => (DayEngine)Activator.CreateInstance(x)!)
            .Where(x => DateTime.Today >= new DateTime(x.Year, 12, x.Day))
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