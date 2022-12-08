using AdventOfCode.Library;

    namespace AdventOfCode._2022;

    public class Day8 : DayEngine
    {
        public override string[] TestInput => new string[]
        {
            "30373",
            "25512",
            "65332",
            "33549",
            "35390",
        };

        protected override object HandlePart1(string[] input)
        {
            var grid = Grid<int>.Create(input, x => int.Parse(x.ToString()));

            var visibleCount = ((grid.Size - 2) * 2) + (grid.RowSize(0) * 2);

            for (int y = 1; y < grid.Size - 1; y++)
            {
                for (int x = 1; x < grid.RowSize(y) - 1; x++)
                {
                    var (canSee, _) = IsVisibleAndScore(grid, x, y);
                    if (canSee)
                    {
                        visibleCount += 1;
                    }
                }
            }

            return visibleCount;
        }

        protected override object HandlePart2(string[] input)
        {
            var grid = Grid<int>.Create(input, x => int.Parse(x.ToString()));

            var maxScore = 0;

            for (int y = 1; y < grid.Size - 1; y++)
            {
                for (int x = 1; x < grid.RowSize(y) - 1; x++)
                {
                    var (_, score) = IsVisibleAndScore(grid, x, y);
                    maxScore = Math.Max(maxScore, score);
                }
            }

            return maxScore;
        }

        private (bool, int) IsVisibleAndScore(Grid<int> grid, int x, int y)
        {
            var (canSee, score) = IsRowVisible(grid, x, y);
            var (canSee2, score2) = IsColumnVisible(grid, x, y);

            return (canSee || canSee2, score * score2);
        }

        private (bool, int) IsRowVisible(Grid<int> grid, int x, int y)
        {
            var value = grid[y, x];
            var row = grid.GetRow(y);
            return CanSeeAndScore(row, value, x);
        }
        
        private (bool, int) IsColumnVisible(Grid<int> grid, int x, int y)
        {
            var value = grid[y, x];
            var column = grid.GetColumn(x);
            return CanSeeAndScore(column, value, y);
        }

        private (bool, int) CanSeeAndScore(int[] rowCol, int value, int gridPos)
        {
            var canSee1 = true;
            var canSee2 = true;

            var score1 = 0;
            var score2 = 0;

            for (int i = gridPos - 1; i >= 0; i--)
            {
                score1++;
                if (rowCol[i] >= value)
                {
                    canSee1 = false;
                    break;
                }
            }

            for (int i = gridPos + 1; i < rowCol.Length; i++)
            {
                score2++;
                if (rowCol[i] >= value)
                {
                    canSee2 = false;
                    break;
                }
            }

            return (canSee1 || canSee2, score1 * score2);
        }
    }