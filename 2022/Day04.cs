using System.Buffers;
using AdventOfCode.Library;

    namespace AdventOfCode._2022;

    public class Day4 : DayEngine
    {
        public override string[] TestInput => new string[]
        {
            "2-4,6-8",
            "2-3,4-5",
            "5-7,7-9",
            "2-8,3-7",
            "6-6,4-6",
            "2-6,4-8",
        };

        protected override object HandlePart1(string[] input)
        {
            var count = 0;
            foreach (var pair in input.Select(x => x.Split(',')))
            {
                var f1 = pair[0].Split('-');
                var f2 = pair[1].Split('-');
                var first = Enumerable.Range(int.Parse(f1[0].ToString()), int.Parse(f1[1].ToString()) - int.Parse(f1[0].ToString()) + 1).ToArray();
                var second = Enumerable.Range(int.Parse(f2[0].ToString()), int.Parse(f2[1].ToString()) - int.Parse(f2[0].ToString()) + 1).ToArray();

                if (first.AsSpan().IndexOfAnyExcept(second) == -1 || second.AsSpan().IndexOfAnyExcept(first) == -1)
                {
                    count += 1;
                }
            }
            return count;
        }

        protected override object HandlePart2(string[] input)
        {
            var count = 0;
            foreach (var pair in input.Select(x => x.Split(',')))
            {
                var f1 = pair[0].Split('-');
                var f2 = pair[1].Split('-');
                var first = Enumerable.Range(int.Parse(f1[0].ToString()), int.Parse(f1[1].ToString()) - int.Parse(f1[0].ToString()) + 1).ToArray();
                var second = Enumerable.Range(int.Parse(f2[0].ToString()), int.Parse(f2[1].ToString()) - int.Parse(f2[0].ToString()) + 1).ToArray();

                if (first.AsSpan().IndexOfAny(second) != -1 || second.AsSpan().IndexOfAny(first) != -1)
                {
                    count += 1;
                }
            }
            return count;
        }
    }