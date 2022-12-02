// See https://aka.ms/new-console-template for more information

using AdventOfCode;

Console.WriteLine("Starting!");

var dayRunner = new DayRunner();

await dayRunner.GenerateMissing(2022, @"D:\aoc\");
await dayRunner.Run(2022);
