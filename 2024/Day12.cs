using AdventOfCode.Library;

namespace AdventOfCode._2024;

public class Day12 : DayEngine
{
    private int nextId = 0;

    private int NextId()
    {
        return nextId++;
    }
    
    // public override string[] TestInput =>
    // [
    //     "AAAA",
    //     "BBCD",
    //     "BBCC",
    //     "EEEC"
    // ];
    
    // public override string[] TestInput =>
    // [
    //     "OOOOO",
    //     "OXOXO",
    //     "OOOOO",
    //     "OXOXO",
    //     "OOOOO",
    // ];

    // public override string[] TestInput =>
    // [
    //     "EEEEE",
    //     "EXXXX",
    //     "EEEEE",
    //     "EXXXX",
    //     "EEEEE",
    // ];

    // public override string[] TestInput =>
    // [
    //     "EEEEE",
    //     "EXXXX",
    //     "EEEEE",
    //     "EXXXX",
    //     "EEEEE",
    // ];
    
    
    // public override string[] TestInput =>
    // [
    //     "AAAAAA",
    //     "AAABBA",
    //     "AAABBA",
    //     "ABBAAA",
    //     "ABBAAA",
    //     "AAAAAA",
    // ];
    //
    public override string[] TestInput =>
    [
        "RRRRIICCFF",
        "RRRRIICCCF",
        "VVRRRCCFFF",
        "VVRCCCJFFF",
        "VVVVCJJCFE",
        "VVIVCCJJEE",
        "VVIIICJJEE",
        "MIIIIIJJEE",
        "MIIISIJEEE",
        "MMMISSJEEE",
    ];
    
    protected override object HandlePart1(string[] input)
    {
        var grid = Grid<GridRegion>.Create(input, c => new GridRegion(c));
        var regions = new Dictionary<int, Region>();
        
        for (int y = 0; y < grid.Size; y++)
        {
            for (int x = 0; x < grid.RowSize(y); x++)
            {
                FloodFill(grid, new Point(x, y), regions);
            }
        }
        
        return regions.Select(x => x.Value.Value()).Sum();
    }

    protected override object HandlePart2(string[] input)
    {
        var grid = Grid<GridRegion>.Create(input, c => new GridRegion(c));
        var regions = new Dictionary<int, Region>();
        
        for (int y = 0; y < grid.Size; y++)
        {
            for (int x = 0; x < grid.RowSize(y); x++)
            {
                FloodFill(grid, new Point(x, y), regions);
            }
        }
        
        // Top Down
        Sides(grid, new Point(0, 0), new Vector(0, 1),  regions);
        // // Bottom up
        Sides(grid, new Point(0, grid.Size - 1), new Vector(0, -1), regions);
        // Left right
        Sides(grid, new Point(0, 0), new Vector(1, 0), regions);
        // Right left
        Sides(grid, new Point(grid.RowSize() - 1, 0), new Vector(-1, 0), regions);
        
        return regions.Select(x => x.Value.Value2()).Sum();
    }

    void FloodFill(Grid<GridRegion> grid, Point current, Dictionary<int, Region> regions)
    {
        if (grid[current]!.RegionId.HasValue) return;

        var regionId = NextId();
        var region = regions.GetOrAdd(regionId, () => new Region(grid[current]!.Id, regionId));
        
        var visited = new HashSet<Point>() { current };
        var neighbours = new Queue<Point>();
        neighbours.Enqueue(current);

        while (neighbours.TryDequeue(out var next))
        {
            region.Area += 1;
            current = next;
            
            var pointRegion = grid[current]!;
            pointRegion.RegionId = regionId;
            
            foreach (var surrounding in Vector.GenerateStraightDirectionalForGridUnchecked(1, current, grid)
                         .Select(x => current + x))
            {
                if (!grid.IsPointWithinBounds(surrounding) || grid[surrounding]!.Id != region.Id)
                {
                    region.Borders += 1;
                } else if (grid[surrounding]!.Id == region.Id)
                {
                    if (visited.Add(surrounding))
                    {
                        neighbours.Enqueue(surrounding);
                    }
                }
            }
        }
    }

    void Sides(Grid<GridRegion> grid, Point start, Vector direction, Dictionary<int, Region> regions)
    {
        var blockDirection = direction.Inverse().Abs();
        while (grid.IsPointWithinBounds(start))
        {
            var current = start;
            while (grid.IsPointWithinBounds(current))
            {
                var source = current - direction;
                var sourceRegion = grid[source];
                var sideRegion = grid[current]!.RegionId!.Value;
                // Did we flip region
                if (sourceRegion == null || sourceRegion.RegionId != sideRegion)
                {
                    // while is same side
                    while (grid.IsPointWithinBounds(current))
                    {
                        var currentRegion = grid[current]!.RegionId!.Value;
                        sourceRegion = grid[current - direction];
                        if (currentRegion != sideRegion || currentRegion == sourceRegion?.RegionId) break;
                        current += blockDirection;
                    }

                    regions[sideRegion].Sides += 1;
                }
                else
                {
                    current += blockDirection;
                }
            }

            start += direction;
        }
    }
}

record GridRegion(char Id)
{
    public int? RegionId { get; set; }
    
}

record Region(char Id, int RegionId)
{
    public int Borders { get; set; }
    public int Sides { get; set; }
    public int Area { get; set; }

    public int Value() => Borders * Area;
    public int Value2() => Sides * Area;
}