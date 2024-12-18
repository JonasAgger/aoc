using AdventOfCode;

Console.WriteLine("Starting!");

var dayRunner = new DayRunner(GetProjectDir());

await dayRunner.GenerateMissing(DateTime.Today.Year);
await dayRunner.RunTimed(2024, 18);
// await dayRunner.Run(2024, current: true);


string GetProjectDir()
{
    return args.ElementAtOrDefault(0) ?? Directory.GetParent(Environment.CurrentDirectory)!.Parent!.Parent!.FullName;
}