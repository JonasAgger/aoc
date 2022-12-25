using AdventOfCode.Library;

namespace AdventOfCode._2022;

public class Day18 : DayEngine
{
    public override string[] TestInput => new string[]
    {
        "2,2,2",
        "1,2,2",
        "3,2,2",
        "2,1,2",
        "2,3,2",
        "2,2,1",
        "2,2,3",
        "2,2,4",
        "2,2,6",
        "1,2,5",
        "3,2,5",
        "2,1,5",
        "2,3,5",
    };

    protected override object HandlePart1(string[] input)
    {
        var tensor = Tensor.Create(input, str =>
        {
            var parts = str.Split(',');
            return new Voxel(parts[0].Int(), parts[1].Int(), parts[2].Int());
        });


        var count = tensor.GetVoxels().Select(x => 6 - tensor.GetNeighbourCount(x)).Sum();
        
        return count;
    }

    protected override object HandlePart2(string[] input)
    {
        var tensor = TensorWithBoundary.Create(input, str =>
        {
            var parts = str.Split(',');
            return new Voxel(parts[0].Int(), parts[1].Int(), parts[2].Int());
        });


        var floodFill = tensor.GetFloodFillVoxels();
        var count = tensor
            .GetVoxels()
            .Select(x => 6 - tensor.GetNeighbourCount2(x, floodFill))
            .Sum();
        // var count = tensor.GetConnectedVoxels().Select(x => 6 - tensor.GetNeighbourCount(x)).Sum();
        
        return count;
    }
}