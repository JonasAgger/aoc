using AdventOfCode.Library;
using Disruptor;
using Disruptor.Dsl;

namespace AdventOfCode._2024;

    public class Day4 : DayEngine
    {
        public override string[] TestInput => new string[]
        {
            "MMMSXXMASM",
            "MSAMXMSMSA",
            "AMXSXMAAMM",
            "MSAMASMSMX",
            "XMASAMXAMM",
            "XXAMMXXAMA",
            "SMSMSASXSS",
            "SAXAMASAAA",
            "MAMMMXMMMM",
            "MXMXAXMASX",
        };

        protected override object HandlePart1(string[] input)
        {
            var grid = Grid<char>.Create(input, c => c);

            var sum = grid.IndexOfAll('X').Select(point =>
            {
                var vectors = Vector.Generate8DirectionalForGrid(3, point, grid);
                return vectors.Count(vec => Matches(point, vec, grid));
            }).Sum();
           
            
            return sum;
        }

        protected override object HandlePart2(string[] input)
        {
            var grid = Grid<char>.Create(input, c => c);

            var sum = grid.IndexOfAll('A').Where(point =>
            {
                var vectors = Vector.GenerateDiagonalDirectionalForGrid(1, point, grid);
                // Basically due to the problem being an X around the current point, then all diagonal points around the current point needs to be valid.
                if (vectors.Count != 4)
                {
                    return false;
                }
                
                // We need 2 of the vectors to match to make a CROSS (because a match is point +/- vec) making an / or \
                return vectors.Count(vec => Matches2(point, vec, grid)) == 2;
            }).Count();
           
            
            return sum;
        }

        bool Matches(Point p, Vector vector, Grid<char> grid)
        {
            var point = p + vector;
            if (!grid[point].Equals('M'))
            {
                return false;
            }
            point += vector;
            if (!grid[point].Equals('A'))
            {
                return false;
            }
            point += vector;
            if (!grid[point].Equals('S'))
            {
                return false;
            }

            return true;
        }
        
        bool Matches2(Point p, Vector vector, Grid<char> grid)
        {
            var point1 = p + vector;
            var point2 = p - vector;

            return grid[point1].Equals('M') && grid[point2].Equals('S');
        }
        
    }