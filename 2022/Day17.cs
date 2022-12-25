using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using AdventOfCode.Library;

    namespace AdventOfCode._2022;

    public class Day17 : DayEngine
    {
        private const bool Debug = false;
        public override string[] TestInput => new string[]
        {
            ">>><<><>><<<>><>>><<<>>><<<><<<>><>><<>>"
        };

        protected override object HandlePart1(string[] input)
        {
            var jetStream = new JetStream(input.First());
            var turn = 0L;
            var rockCount = 0L;
            var maxRockCount = 2023L;

            var stones = new List<List<Point>>();
            stones.Add(new List<Point>()
            {
                new Point(0,0),
                new Point(1,0),
                new Point(2,0),
                new Point(3,0),
                new Point(4,0),
                new Point(5,0),
                new Point(6,0),
            });
            
            while (rockCount < (maxRockCount - 1)) // -1 Since index 0 counts as well.
            {
                rockCount = RockCount(rockCount, stones, jetStream, ref turn);
            }
            
            
            
            return stones.Count - 1; // Dont account for the floor
        }

        private long RockCount(long rockCount, List<List<Point>> stones, JetStream jetStream, ref long turn)
        {
            var rock = Rock.GenerateRock(rockCount, stones.Count);
            var direction = GetDirection(turn, jetStream);
            Print(stones, rock);

            // We only care if it cant move while going down, therefor if it cant move, dont check.
            // If it cant move, check that we're not going downwards
            while (rock.Move(direction, stones) || IsJetAffected(turn))
            {
                turn += 1;
                direction = GetDirection(turn, jetStream);

                Print(stones, rock);
            }

            turn += 1;
            rockCount += 1;
            rock.Add(stones);
            return rockCount;
        }

        protected override object HandlePart2(string[] input)
        {
            var jetStream = new JetStream(input.First());
            var turn = 0L;
            var rockCount = 0L;
            var maxRockCount = 1_000_000_000_000L;

            var stones = new List<List<Point>>();
            stones.Add(new List<Point>()
            {
                new Point(0,0),
                new Point(1,0),
                new Point(2,0),
                new Point(3,0),
                new Point(4,0),
                new Point(5,0),
                new Point(6,0),
            });
            var repeats = new Dictionary<State, HeightKey>();
            
            
            while (rockCount < (maxRockCount - 1)) // -1 Since index 0 counts as well.
            {
                var rock = Rock.GenerateRock(rockCount, stones.Count);
                var direction = GetDirection(turn, jetStream);
                Print(stones, rock);

                // We only care if it cant move while going down, therefor if it cant move, dont check.
                // If it cant move, check that we're not going downwards
                while (rock.Move(direction, stones) ||  IsJetAffected(turn)) 
                {
                    turn += 1;
                    direction = GetDirection(turn, jetStream);

                    Print(stones, rock);
                }
                
                turn += 1;
                rockCount += 1;
                rock.Add(stones);
                
                                // Just omit the first couple ones.
                if (stones.Count > FloorsToHash)
                {
                    var s = State.Create(rock, jetStream, stones);
                    var k = new HeightKey(turn, rockCount, stones.Count - 1);
                    if (!repeats.TryAdd(s, k)) // Find a pattern, then assume that the pattern repeats.
                    {
                        if (true)
                        // if (Debug)
                        {
                            Console.WriteLine($"repeat of {s} - {repeats[s]}");
                            Console.WriteLine($"repeat of {s} - {k}");
                        }
                        var turnDiff = k.Turn - repeats[s].Turn;
                        var heightDiff = k.Height - repeats[s].Height;
                        var rockDiff = k.Rocks - repeats[s].Rocks;

                        var remainderRocks = maxRockCount - k.Rocks;
                        var multiples = remainderRocks / rockDiff;
                        
                        var heightAddition = multiples * heightDiff;
                        var turnAddition = multiples * turnDiff;
                        var rockAddition = multiples * rockDiff;

                        rockCount += rockAddition;
                        turn += turnAddition;
                    
                        while (rockCount < (maxRockCount)) // NOT -1 here because we forget to count the NEWEST rock when adding all.
                        {
                            rockCount = RockCount(rockCount, stones, jetStream, ref turn);
                        }

                        return stones.Count - 1 + heightAddition;
                    }
                }
            }

            throw new UnreachableException();
        }

        
        private unsafe void Print(List<List<Point>> stones, Rock rock)
        {
            if (!Debug) return;
            var maxY = Math.Max(rock.Points.Max(x => x.Y), stones.Count);

            var canvas = new string[maxY + 1];
            canvas[0] = "|-------|";
            for (int i = 1; i < canvas.Length; i++)
            {
                canvas[i] = new string("|.......|");
            }

            for (int i = 1; i < stones.Count; i++)
            {
                foreach (var p in stones[i])
                {
                    fixed (char* ptr = &canvas[i].GetPinnableReference())
                    {
                        Unsafe.Write((ptr + p.X + 1), '#');
                    }
                }
            }

            foreach (var p in rock.Points)
            {
                fixed (char* ptr = &canvas[p.Y].GetPinnableReference())
                {
                    Unsafe.Write((ptr + p.X+ 1), '@');
                }
            }

            for (int i = canvas.Length - 1; i >= 0; i--)
            {
                Console.WriteLine(canvas[i]);
            }
            Console.WriteLine();
        }

        private Point GetDirection(long turn, JetStream jetStream) => turn % 2 == 0 ? new Point(jetStream.Move(), 0) : new Point(0, -1);
        private bool IsJetAffected(long turn) => turn % 2 == 0;
        
        private record Rock(Point[] Points, int Type)
        {
            private static Point[][] shapes = new[]
            {
                new[]
                {
                    new Point(0, 0),
                    new Point(1, 0),
                    new Point(2, 0),
                    new Point(3, 0),
                },
                new[]
                {
                    new Point(0, 1),
                    new Point(1, 1),
                    new Point(2, 1),
                    new Point(1, 0),
                    new Point(1, 2),
                },
                new[]
                {
                    new Point(0, 0),
                    new Point(1, 0),
                    new Point(2, 0),
                    new Point(2, 1),
                    new Point(2, 2),
                },
                new[]
                {
                    new Point(0, 0),
                    new Point(0, 1),
                    new Point(0, 2),
                    new Point(0, 3),
                },
                new[]
                {
                    new Point(0, 0),
                    new Point(0, 1),
                    new Point(1, 0),
                    new Point(1, 1),
                },
            };

            public static Rock GenerateRock(long rockCount, int stonesCount)
            {
                var i = GetRockType(rockCount);
                var offset = new Point(2, stonesCount + 3);
                return new Rock(shapes[i].Select(x => x + offset).ToArray(), i);
            }
            
            public static int GetRockType(long rockCount) => (int)(rockCount % shapes.Length);
            public int GetNextRockType() => ((Type + 1) % shapes.Length);
            
            public Point[] Points { get; private set; } = Points;
            
            public bool Move(Point delta, List<List<Point>> stones)
            {
                var newPoints = new Point[Points.Length];

                for (int i = 0; i < Points.Length; i++)
                {
                    var p = Points[i] + delta;

                    if (p.X is < 0 or > 6)
                    {
                        if (Debug) Console.WriteLine($"Cannot move {delta}, {p}");
                        return false;
                    }

                    if (stones.TryGet(p.Y, out var floor) && floor.Any(x => x.X == p.X))
                    {
                        if (Debug) Console.WriteLine($"Cannot move {delta} due to floor {p.Y} --- {string.Join(", ", floor.Select(x => x.X))}");
                        return false;
                    }
                    newPoints[i] = p;
                }

                Points = newPoints;
                return true;
            }

            public void Add(List<List<Point>> stones)
            {
                foreach (var p in Points)
                {
                    while(p.Y >= stones.Count)
                    {
                        stones.Add(new List<Point>());
                    }
                    
                    stones[p.Y].Add(p);
                }
            }
        }
        
        private record JetStream(string Sequence)
        {
            public int Index { get; private set; } = 0;

            public int Move()
            {
                var i = Index;
                Index = (Index + 1) % Sequence.Length;
                return Sequence[i] switch
                {
                    '<' => -1,
                    '>' => 1,
                    _ => throw new UnreachableException()
                };
            }
        }


        private const int FloorsToHash = 50;
        private record State(string BitMap, int JetStreamIndex, int RockIndex)
        {
            public static State Create(Rock rock, JetStream js, List<List<Point>> stones)
            {
                var bitmap = new StringBuilder();
                for (int i = 1; i <= FloorsToHash; i++)
                {
                    var floor = stones[^i];

                    var floorBitmap = GenerateFloorBitMap(floor);
                    
                    bitmap.Append(floorBitmap);
                }
                
                return new State(bitmap.ToString(), js.Index, rock.Type);
            }

            private static int GenerateFloorBitMap(List<Point> floor)
            {
                var bitmap = 0;

                foreach (var p in floor)
                {
                    bitmap |= 1 << p.X;
                }

                return bitmap;
            }
        }

        private record HeightKey(long Turn, long Rocks, long Height);
    }