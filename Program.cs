using AdventOfCode;

Console.WriteLine("Starting!");

var dayRunner = new DayRunner();

var projectDirectory = GetProjectDir();
await dayRunner.GenerateMissing(2024, projectDirectory);
await dayRunner.Run(2024);


string GetProjectDir()
{
    return args.ElementAtOrDefault(0) ?? Directory.GetParent(Environment.CurrentDirectory)!.Parent!.Parent!.FullName;
}