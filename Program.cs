using AdventOfCode;

Console.WriteLine("Starting!");

var dayRunner = new DayRunner();

var projectDirectory = Directory.GetParent(Environment.CurrentDirectory)!.Parent!.Parent!.FullName;
await dayRunner.GenerateMissing(2022, projectDirectory);
await dayRunner.RunTimed(2022);