namespace AdventOfCode.Library;

public class TensorWithBoundary
{
    private readonly Voxel[] grid;
    private readonly int maxX;
    private readonly int maxY;
    private readonly int maxZ;
    private readonly int maxIndex;
    private readonly int minIndex;

    private TensorWithBoundary(Voxel[] grid, int maxX, int maxY, int maxZ)
    {
        this.grid = grid;
        this.maxX = maxX;
        this.maxY = maxY;
        this.maxZ = maxZ;
        maxIndex = GetIndex(new Voxel(maxX - 2, maxY - 2, maxZ - 2));
        minIndex = GetIndex(new Voxel(0,0,0));
    }

    public static TensorWithBoundary Create(string[] input, Func<string, Voxel> generator)
    {
        var voxels = input
            .Select(generator)
            .ToList();

        var maxX = voxels.Max(x => x.X) + 2;
        var maxY = voxels.Max(x => x.Y) + 2;
        var maxZ = voxels.Max(x => x.Z) + 2;

        var grid = new Voxel[maxZ * maxY * maxX];
        var tensor = new TensorWithBoundary(grid, maxX, maxY, maxZ);
        
        foreach (var voxel in voxels)
        {
            tensor.Insert(voxel);
        }

        return tensor;
    }

    private void Insert(Voxel voxel)
    {
        var index = GetIndex(voxel);
        grid[index] = voxel;
    }

    private int GetIndex(Voxel voxel)
    {
        var zOffset = (voxel.Z +1) * (maxY * maxX);
        var yOffset = maxX * (voxel.Y + 1);
        return zOffset + yOffset + (voxel.X + 1);
    }

    private bool IsValid(int index) => index < minIndex || index >= maxIndex;
    private bool IsValid(Voxel voxel)
    {
        if (voxel.X < -1 || voxel.X > maxX) return false;
        if (voxel.Y < -1 || voxel.Y > maxY) return false;
        if (voxel.Z < -1 || voxel.Z > maxZ) return false;
        return true;
    }
    
    private bool IsValid2(Voxel voxel)
    {
        if (voxel.X < 0 || voxel.X >= maxX) return false;
        if (voxel.Y < 0 || voxel.Y >= maxY) return false;
        if (voxel.Z < 0 || voxel.Z >= maxZ) return false;
        return true;
    }

    public int GetNeighbourCount(Voxel voxel)
    {
        var possibleNeighbours = new[]
        {
            new Voxel(-1, 0, 0),
            new Voxel(1, 0, 0),
            new Voxel(0, -1, 0),
            new Voxel(0, 1, 0),
            new Voxel(0, 0, -1),
            new Voxel(0, 0, 1),
        };

        var neighbourCount = 0;
        
        foreach (var pn in possibleNeighbours)
        {
            var index = GetIndex(voxel + pn);

            if (IsValid(index))
            {
                continue;
            }
            
            if (grid[index].Created) neighbourCount += 1;
        }

        return neighbourCount;
    }
    
    public int GetNeighbourCount2(Voxel voxel, HashSet<Voxel> seenVoxels)
    {
        var possibleNeighbours = new[]
        {
            new Voxel(-1, 0, 0),
            new Voxel(1, 0, 0),
            new Voxel(0, -1, 0),
            new Voxel(0, 1, 0),
            new Voxel(0, 0, -1),
            new Voxel(0, 0, 1),
        };

        var neighbourCount = 0;
        
        foreach (var pn in possibleNeighbours)
        {
            var tvoxel = voxel + pn;
            var index = GetIndex(tvoxel);

            if (IsValid(index))
            {
                continue;
            }

            if (grid[index].Created) neighbourCount += 1;
            else if (!seenVoxels.Contains(voxel + pn))
            {
                neighbourCount += 1;
            }
        }

        return neighbourCount;
    }
    
    public IEnumerable<Voxel> GetVoxels() => grid.Where(x => x.Created);

    public HashSet<Voxel> GetFloodFillVoxels()
    {
        var outerVoxels = new[]
        {
            new Voxel(-1,-1,-1),
            new Voxel(maxX,-1, -1),
            new Voxel(maxX,maxY,-1),
            new Voxel(maxX,maxY,maxZ),
            new Voxel(-1,maxY,maxZ),
            new Voxel(-1,-1,maxZ),
        }.Where(x => GetIndex(x) >= grid.Length || !grid[GetIndex(x)].Created)
            .ToList();
        
        var queue = new Queue<Voxel>();
        var seenVoxels = new HashSet<Voxel>();

        foreach (var v in outerVoxels)
        {
            queue.Enqueue(v);
            seenVoxels.Add(v);
        }

        var possibleNeighbours = new[]
        {
            new Voxel(-1, 0, 0),
            new Voxel(1, 0, 0),
            new Voxel(0, -1, 0),
            new Voxel(0, 1, 0),
            new Voxel(0, 0, -1),
            new Voxel(0, 0, 1),
        };
        
        while (queue.TryDequeue(out var current))
        {
            foreach (var pn in possibleNeighbours)
            {
                var index = GetIndex(current + pn);

                if (IsValid(index))
                {
                    continue;
                }

                var voxel = grid[index];
                if (!voxel.Created)
                {
                    voxel = current + pn;
                    
                    if (IsValid(voxel) && seenVoxels.Add(voxel))
                    {
                        queue.Enqueue(voxel);
                    }
                }
            }
        }

        return seenVoxels;
    }
}