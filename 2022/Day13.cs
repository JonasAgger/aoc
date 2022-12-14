using System.Diagnostics;
using AdventOfCode.Library;

    namespace AdventOfCode._2022;

    public class Day13 : DayEngine
    {
        public override string[] TestInput => new string[]
        {
            "[1,1,3,1,1]",
            "[1,1,5,1,1]",
            "[[1],[2,3,4]]",
            "[[1],4]",
            "[9]",
            "[[8,7,6]]",
            "[[4,4],4,4]",
            "[[4,4],4,4,4]",
            "[7,7,7,7]",
            "[7,7,7]",
            "[]",
            "[3]",
            "[[[]]]",
            "[[]]",
            "[1,[2,[3,[4,[5,6,7]]]],8,9]",
            "[1,[2,[3,[4,[5,6,0]]]],8,9]",
        };

        protected override object HandlePart1(string[] input)
        {
            var groups = input.Chunk(2).Select(Parse).ToList();

            var correctPairs = 0;

            for (int i = 0; i < groups.Count; i++)
            {
                if (IsMatched(groups[i].Item1, groups[i].Item2) == Match.Right)
                {
                    correctPairs += i + 1;
                }
            }
            
            return correctPairs;
        }
        
        protected override object HandlePart2(string[] input)
        {
            var i1 = new ListItem(new List<Item>() { new ListItem(new List<Item>() { new SingleItem(2) }) });
            var i2 = new ListItem(new List<Item>() { new ListItem(new List<Item>() { new SingleItem(6) }) });
            
            var items = input
                .Chunk(2)
                .Select(Parse)
                .SelectMany(x => new[]{x.Item1, x.Item2})
                .Concat(new []
                    {
                        i1,
                        i2
                    })
                .ToList();
            
            items.Sort(Sort);

            var ix1 = items.IndexOf(i1) + 1;
            var ix2 = items.IndexOf(i2) + 1;
            
            return ix1 * ix2;
        }

        private static int Sort(Item item1, Item item2) => IsMatched(item1, item2) switch
        {
            Match.Right => -1,
            Match.Wrong => 1,
            Match.Equal => 0,
            _ => throw new ArgumentOutOfRangeException()
        };
        private static Match IsMatched(Item item1, Item item2)
        {
            if (item1 is SingleItem s1 && item2 is SingleItem s2)
            {
                if (s1.i == s2.i) return Match.Equal;
                if (s1.i > s2.i) return Match.Wrong;
                if (s1.i < s2.i) return Match.Right;
            }
            else if (item1 is ListItem l1 && item2 is ListItem l2)
            {
                var l2Bound = l2.ints.Count;
                
                for (int i = 0; i < l1.ints.Count; i++)
                {
                    if (i >= l2Bound) return Match.Wrong;

                    var comp = IsMatched(l1.ints[i], l2.ints[i]);
                    if (comp != Match.Equal) return comp;
                }

                if (l2Bound > l1.ints.Count) return Match.Right;
                return Match.Equal;
            }
            else
            {
                if (item1 is SingleItem st1) return IsMatched(new ListItem(new List<Item>() { st1 }), item2);
                if (item2 is SingleItem st2) return IsMatched(item1, new ListItem(new List<Item>() { st2 }));
            }

            throw new UnreachableException();
        }
        
        



        private (ListItem, ListItem) Parse(string[] input)
        {
            var s1= input[0].AsSpan();
            var s2= input[1].AsSpan();
            var baseItem1 = (ListItem)Parse(ref s1);
            var baseItem2 = (ListItem)Parse(ref s2);

            return (baseItem1, baseItem2);
        }

        private Item Parse(ref ReadOnlySpan<char> input)
        {
            if (input[0] == ',') input = input.Slice(1);
            
            if (input[0] == '[')
            {
                input = input.Slice(1);
                
                var items = new List<Item>();
                
                while (!input.IsEmpty && input[0] != ']')
                {
                    items.Add(Parse(ref input));
                }

                if (!input.IsEmpty) input = input.Slice(1);
                
                return new ListItem(items);
            }
            else
            {
                var intEnd = input.IndexOfAny(']', ',');
                var i = int.Parse(input.Slice(0, intEnd));
                input = input.Slice(intEnd);
                return new SingleItem(i);
            }
        }

        enum Match
        {
            Right,
            Wrong, 
            Equal
        }
        
        private abstract record Item();

        private record ListItem(List<Item> ints) : Item
        {
            public override string ToString() => $"[{string.Join(",", ints)}]";
        }

        private record SingleItem(int i) : Item
        {
            public override string ToString() => i.ToString();
        }
    }