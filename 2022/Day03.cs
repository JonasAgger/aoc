using System.Diagnostics;
using AdventOfCode.Library;

    namespace AdventOfCode._2022;

    public class Day3 : DayEngine
    {
        public override string[] TestInput => new string[]
        {
            "vJrwpWtwJgWrhcsFMMfFFhFp",
            "jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL",
            "PmmdzqPrVvPwwTWBwg",
            "wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn",
            "ttgJtRGJQctTZtZT",
            "CrZsJsPPZsGzwwsLwLmpwMDw",
        };

        protected override object HandlePart1(string[] input)
        {
            var commonItems = new char[input.Length];
            
            foreach (var (bag, index) in input.Select((x,i) => (x,i)))
            {
                commonItems[index] = CommonItem(bag);
            }
            
            return commonItems.Sum(CalculateValue);
        }

        protected override object HandlePart2(string[] input)
        {
            var commonItems = new char[input.Length / 3];
            
            foreach (var (bags, index) in input.Chunk(3).Select((x,i) => (x,i)))
            {
                commonItems[index] = CommonItem(bags);
            }
            
            return commonItems.Sum(CalculateValue);
        }

        private int CalculateValue(char item)
        {
            if (Char.IsLower(item)) return item - 'a' + 1;
            return item - 'A' + 27;
        }
        
        private char CommonItem(string bag)
        {
            var set = new bool[127]; // ASCII range.
            var compartment1 = bag.AsSpan(0, bag.Length / 2);
            var compartment2 = bag.AsSpan(bag.Length / 2);

            foreach (var item in compartment1)
            {
                set[item] = true;
            }

            foreach (var item in compartment2)
            {
                if (set[item]) return item;
            }

            throw new UnreachableException();
        }
        
        private char CommonItem(string[] bags)
        {
            var set = new byte[127]; // ASCII range.

            foreach (var bag in bags)
            {
                var bagSet = new bool[127]; // ASCII range.

                foreach (var item in bag.AsSpan())
                {
                    if (!bagSet[item])
                    {
                        bagSet[item] = true;
                        set[item] += 1;
                    }
                }
            }

            return (char)set.AsSpan().IndexOf((byte)3);
        }
    }