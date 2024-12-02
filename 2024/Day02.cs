using AdventOfCode.Library;

    namespace AdventOfCode._2024;

    public class Day2 : DayEngine
    {
        public override string[] TestInput => new string[]
        {
            "7 6 4 2 1",
            "1 2 7 8 9",
            "9 7 6 2 1",
            "1 3 2 4 5",
            "8 6 4 4 1",
            "1 3 6 7 9",
        };

        protected override object HandlePart1(string[] input)
        {
            return input.Count(s =>
            {
                var numbers = s.Split().Select(int.Parse).ToList();
                return Check(numbers);
            });
        }

        protected override object HandlePart2(string[] input)
        {
            return input.Count(s =>
            {
                var numbers = s.Split().Select(int.Parse).ToList();
                if (Check(numbers))
                {
                    return true;
                }
                // Remove an element from the list and just check if it's valid
                return Enumerable.Range(0, numbers.Count).Where(nr =>
                {
                    var newList = new List<int>(numbers);
                    newList.RemoveAt(nr);
                    return Check(newList);
                }).Any();
            });
        }

        bool Check(List<int> numbers)
        {
            bool? asc = null;

            for (int i = 1; i < numbers.Count; i++)
            {
                // Check if we've moving up or down.
                if (i == 1)
                {
                    asc = numbers[i - 1] < numbers[i];
                }
                    
                if (asc!.Value && numbers[i-1] < numbers[i])
                {
                    if (numbers[i] - numbers[i - 1] > 3) return false;
                }
                else if (!asc!.Value && numbers[i - 1] > numbers[i])
                {
                    if (numbers[i - 1] - numbers[i] > 3) return false;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }
    }