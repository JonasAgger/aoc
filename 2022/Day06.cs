using AdventOfCode.Library;

    namespace AdventOfCode._2022;

    public class Day6 : DayEngine
    {
        public override string[] TestInput => new string[]
        {
            "mjqjpqmgbljsphdztnvjfqwrcgsmlb",
        };

        protected override object HandlePart1(string[] input)
        {
            var inputStr = input[0].AsSpan();

            for (int i = 0; i < inputStr.Length; i++)
            {
                if (IsDifferent(inputStr.Slice(i, 4))) return i + 4;
            }
            
            return -1;
        }

        protected override object HandlePart2(string[] input)
        {
            var inputStr = input[0].AsSpan();

            for (int i = 0; i < inputStr.Length; i++)
            {
                if (IsDifferent(inputStr.Slice(i, 14))) return i + 14;
            }
            
            return -1;
        }

        private bool IsDifferent(ReadOnlySpan<char> chars)
        {
            for (int i = 0; i < chars.Length - 1; i++)
            {
                if (chars.Slice(i + 1).IndexOf(chars[i]) != -1) return false;
            }

            return true;
        }
    }