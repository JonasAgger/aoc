using AdventOfCode;

Console.WriteLine("Starting!");

var dayRunner = new DayRunner(GetProjectDir());

await dayRunner.GenerateMissing(DateTime.Today.Year);
await dayRunner.Run(2024, current: false, testMode: false);


string GetProjectDir()
{
    return args.ElementAtOrDefault(0) ?? Directory.GetParent(Environment.CurrentDirectory)!.Parent!.Parent!.FullName;
}