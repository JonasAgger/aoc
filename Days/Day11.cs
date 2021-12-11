using AdventOfCode.Library;

namespace AdventOfCode.Days;

public class Day11 : DayEngine
{
    public override string[] TestInput => new string[]
    {
        "5483143223",
        "2745854711",
        "5264556173",
        "6141336146",
        "6357385478",
        "4167524645",
        "2176841721",
        "6882881134",
        "4846848554",
        "5283751526",
    };

    protected override Task<object> HandlePart1(string[] input)
    {
        const int Steps = 100;
        
        var octopusses = Grid<OctoPus>.Create(input, y => new OctoPus(int.Parse(y.ToString())));

        long flashes = 0;

        for (int step = 0; step < Steps; step++)
        {
            long stepFlashes = 0;

            var isSomeoneFlashing = false;
            
            for (int i = 0; i < octopusses.Size; i++)
            {
                for (int j = 0; j < octopusses.Size; j++)
                {
                    var octo = octopusses[i, j];
                    octo.Energy += 1;

                    if (octo.Energy > 9)
                    {
                        Flash(octopusses, j,i, step);
                        stepFlashes += 1;
                        isSomeoneFlashing = true;
                        octo.LastFlashStep = step;
                        octo.Energy = 0;
                    }
                }
            }

            while (isSomeoneFlashing)
            {
                isSomeoneFlashing = false;
                for (int i = 0; i < octopusses.Size; i++)
                {
                    for (int j = 0; j < octopusses.Size; j++)
                    {
                        var octo = octopusses[i, j];

                        if (octo.LastFlashStep != step && octo.Energy > 9)
                        {
                            Flash(octopusses, j,i, step);
                            stepFlashes += 1;
                            isSomeoneFlashing = true;
                            octo.LastFlashStep = step;
                            octo.Energy = 0;
                        }
                    }
                }
            }

            flashes += stepFlashes;
        }
        
        
        return Task.FromResult<object>(flashes);
    }

    protected override Task<object> HandlePart2(string[] input)
    {
        var octopusses = Grid<OctoPus>.Create(input, y => new OctoPus(int.Parse(y.ToString())));

        for (int step = 0; ; step++)
        {
            long stepFlashes = 0;

            var isSomeoneFlashing = false;
            
            for (int i = 0; i < octopusses.Size; i++)
            {
                for (int j = 0; j < octopusses.Size; j++)
                {
                    var octo = octopusses[i, j];
                    octo.Energy += 1;

                    if (octo.Energy > 9)
                    {
                        Flash(octopusses, j,i, step);
                        stepFlashes += 1;
                        isSomeoneFlashing = true;
                        octo.LastFlashStep = step;
                        octo.Energy = 0;
                    }
                }
            }

            while (isSomeoneFlashing)
            {
                isSomeoneFlashing = false;
                for (int i = 0; i < octopusses.Size; i++)
                {
                    for (int j = 0; j < octopusses.Size; j++)
                    {
                        var octo = octopusses[i, j];

                        if (octo.LastFlashStep != step && octo.Energy > 9)
                        {
                            Flash(octopusses, j,i, step);
                            stepFlashes += 1;
                            isSomeoneFlashing = true;
                            octo.LastFlashStep = step;
                            octo.Energy = 0;
                        }
                    }
                }
            }

            if (stepFlashes == 100) return Task.FromResult<object>(step + 1);
            if (step % 10000 == 0) Console.WriteLine(step);
        }
    }

    
    private void Flash(Grid<OctoPus> grid, int x, int y, long currentStep)
    {
        for (int i = y - 1; i < y + 2; i++)
        {
            for (int j = x - 1; j < x + 2; j++)
            {
                if (i == y && j == x) continue;
                
                var octo = grid[i, j];

                if (octo != null && octo.LastFlashStep != currentStep)
                {
                    octo.Energy += 1;
                }
            }
        }
    }
    private record OctoPus(int Energy)
    {
        public int Energy { get; set; } = Energy;
        public long LastFlashStep { get; set; } = -1;
    }
}