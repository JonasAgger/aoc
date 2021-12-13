using AdventOfCode.Library;

namespace AdventOfCode.Days;

public class Day6 : DayEngine
{
    public override string[] TestInput => new string[]
    {
        "3,4,3,1,2"
    };

    protected override ValueTask<object> HandlePart1(string[] input)
    {
        const int days = 80;

        var fish = input[0].Split(',').Select(int.Parse).ToList();


        for (int day = 0; day < days; day++)
        {
            var fishCount = fish.Count;

            for (int i = 0; i < fishCount; i++)
            {
                var currentFish = fish[i];
                currentFish -= 1;
                if (currentFish < 0)
                {
                    currentFish = 6;
                    fish.Add(8);
                }

                fish[i] = currentFish;
            }
        }

        return ValueTask.FromResult<object>(fish.Count);
    }

    protected override ValueTask<object> HandlePart2(string[] input)
    {
        const int replicationDays = 7;
        const int matureDays = 2;
        const int days = 256;

        var fish = input[0].Split(',').Select(int.Parse).ToList();

        var fishies = new long[7];
        var trailing = new long[2];

        foreach (var f in fish)
            fishies[f] += 1;

        for (int i = 0; i < days; i++)
        {
            var index = i % replicationDays;
            var trailingIndex = i % matureDays;

            var currentFish = fishies[index];
            fishies[index] += trailing[trailingIndex];
            trailing[trailingIndex] = currentFish;
        }

        return ValueTask.FromResult<object>(fishies.Sum() + trailing.Sum());
    }
}