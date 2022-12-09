using AdventOfCode.Library;

    namespace AdventOfCode._2022;

    public class Day9 : DayEngine
    {
        public override string[] TestInput => new string[]
        {
            // "R 4",
            // "U 4",
            // "L 3",
            // "D 1",
            // "R 4",
            // "D 1",
            // "L 5",
            // "R 2",
            //
            "R 5",
            "U 8",
            "L 8",
            "D 3",
            "R 17",
            "D 10",
            "L 25",
            "U 20",
        };

        protected override object HandlePart1(string[] input)
        {
            var pointsVisited = new HashSet<Point>();

            var currentHeadPos = new Point(0, 0); 
            var currentTailPos = new Point(0, 0);
            pointsVisited.Add(currentTailPos);
            
            foreach (var cmd in input)
            {
                var dir = cmd[0] switch
                {
                    'R' => new Point(1, 0),
                    'L' => new Point(-1, 0),
                    'U' => new Point(0, 1),
                    'D' => new Point(0, -1),
                };

                var count = cmd.Split(' ').Last().Int();

                for (int i = 0; i < count; i++)
                {
                    currentHeadPos += dir;
                    currentTailPos = MovePoint(currentHeadPos, currentTailPos);
                    pointsVisited.Add(currentTailPos);
                }
            }
            
            return pointsVisited.Count;
        }

        protected override object HandlePart2(string[] input)
        {
            var pointsVisited = new HashSet<Point>();

            var currentHeadPos = new Point(0, 0);
            var tails = Enumerable.Range(0, 9).Select(x => new Point(0, 0)).ToList();
            pointsVisited.Add(tails.Last());
            
            foreach (var cmd in input)
            {
                var dir = cmd[0] switch
                {
                    'R' => new Point(1, 0),
                    'L' => new Point(-1, 0),
                    'U' => new Point(0, 1),
                    'D' => new Point(0, -1),
                };

                var count = cmd.Split(' ').Last().Int();

                for (int i = 0; i < count; i++)
                {
                    currentHeadPos += dir;
                    tails[0] = MovePoint(currentHeadPos, tails[0]);

                    for (int j = 1; j < tails.Count; j++)
                    {
                        tails[j] = MovePoint(tails[j-1], tails[j]);
                    }
                    pointsVisited.Add(tails.Last());
                }
            }
            
            return pointsVisited.Count;
        }

        private Point MovePoint(Point head, Point tail)
        {
            if (tail.EuclidianDistance(head) > 1.5)
            {
                if (tail.X == head.X)
                {
                    if (tail.Y > head.Y)
                    {
                        tail += new Point(0, -1);
                    }
                    else
                    {
                        tail += new Point(0, 1);
                    }
                }
                else if (tail.Y == head.Y)
                {
                    if (tail.X > head.X)
                    {
                        tail += new Point(-1, 0);
                    }
                    else
                    {
                        tail += new Point(1, 0);
                    }
                }
                else
                {
                    var options = new Point[]
                    {
                        tail + new Point(-1, -1),
                        tail + new Point(-1, 1),
                        tail + new Point(1, -1),
                        tail + new Point(1, 1),
                    };
                    tail = options.MinBy(x => head.EuclidianDistance(x));
                }
            }

            return tail;
        }
    }