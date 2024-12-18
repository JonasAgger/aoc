using System.Collections.Concurrent;
using System.Text;
using AdventOfCode.Library;

    namespace AdventOfCode._2024;

    public class Day14 : DayEngine
    {
        private static readonly Variable<int> GridWidth = Variable<int>.Create(11, 101);
        private static readonly Variable<int> GridHeight = Variable<int>.Create(7, 103);
        
        public override string[] TestInput =>
        [
            "p=0,4 v=3,-3",
            "p=6,3 v=-1,-3",
            "p=10,3 v=-1,2",
            "p=2,0 v=2,-1",
            "p=0,0 v=1,3",
            "p=3,0 v=-2,-2",
            "p=7,6 v=-1,-3",
            "p=3,0 v=-1,-2",
            "p=9,3 v=2,3",
            "p=7,3 v=-1,2",
            "p=2,4 v=2,-3",
            "p=9,5 v=-3,-3"
        ];

        protected override object HandlePart1(string[] input)
        {
            var points = input.Select(x => Project(x, 100)).ToList();
            int[] quardrants = [0, 0, 0, 0];
            
            foreach (var point in points)
            {
                var index = 0;
                
                if (point.Y == GridHeight / 2) continue;
                if (point.X == GridWidth / 2) continue;
                
                if (point.Y > GridHeight / 2) index += 2;
                if (point.X > GridWidth / 2) index += 1;
                quardrants[index] += 1;
            }

            return quardrants.Aggregate(1, (acc, next) => acc * next);
        }

        protected override object HandlePart2(string[] input)
        {
            var points = input.Select(Parse).ToList();

            var cycleTime = GridHeight * GridWidth;

            var distances = Enumerable.Range(0, cycleTime).AsParallel().Select(i =>
            {
                var cPoints = points.Select(x =>
                {
                    var (point, vector) = x;
                    point = point + (vector * i);

                    // clamp to grid
                    point.X = point.X % GridWidth;
                    point.Y = point.Y % GridHeight;

                    // align to positive coordinates
                    point.X = point.X < 0 ? GridWidth + point.X : point.X;
                    point.Y = point.Y < 0 ? GridHeight + point.Y : point.Y;
                    return point;
                }).ToList();

                var dist = cPoints.SkipLast(1)
                    .AsParallel()
                    .SelectMany(((point, idx) => cPoints[idx..].Select(p => point.EuclidianDistance(p))))
                    .Average();

                return (dist, i);
            }).ToList();

            distances = distances.OrderBy(x => x.Item1).ToList();

            for (int i = 0; i < 10; i++)
            {
                var time = distances[i].Item2;
                var currPoints = input.Select(x => Project(x, time)).ToList();
            
                var sb = new StringBuilder();
                for (int y = 0; y < GridHeight; y++)
                {
                    for (int x = 0; x < GridWidth; x++)
                    {
                        var sum = currPoints.Count(p => p == new Point(x, y));
                        if (sum == 0) sb.Append(".");
                        else sb.Append(sum);
                    }
            
                    sb.AppendLine();
                }
                
                File.WriteAllText($"./{i}_{time}.txt", sb.ToString());
            }
            
            
            return distances[0].Item2;
        }

        (Point, Vector) Parse(string s)
        {
            var parts = s.Split()
                .SelectMany(x => x.Split('=')
                    .Last()
                    .Split(',')
                    .Select(x => x.Int()))
                .ToList();
            var point = new Point(parts[0], parts[1]);
            var vector = new Vector(parts[2], parts[3]);
            return (point, vector);
        }
        
        Point Project(string s, int timesProjection)
        {
            var (point, vector) = Parse(s);
            point = point + (vector * timesProjection);

            // clamp to grid
            point.X = point.X % GridWidth;
            point.Y = point.Y % GridHeight;

            // align to positive coordinates
            point.X = point.X < 0 ? GridWidth + point.X : point.X;
            point.Y = point.Y < 0 ? GridHeight + point.Y : point.Y;
            
            return point;
        }
    }