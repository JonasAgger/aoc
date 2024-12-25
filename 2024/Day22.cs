using System.Collections.Specialized;
using System.Numerics;
using System.Runtime.InteropServices;
using AdventOfCode.Library;

    namespace AdventOfCode._2024;

    public class Day22 : DayEngine
    {
        public override string[] TestInput => new string[]
        {
            "1",
            "2",
            "3",
            "2024",
        };

        protected override object HandlePart1(string[] input)
        {
            var startingNumbers = input.Select(x => x.Long()).ToList();
            
            
            return startingNumbers.Select(SecretNumber).Sum();
        }

        protected override object HandlePart2(string[] input)
        {
            var startingNumbers = input.Select(x => x.Long()).ToList();
            var cache = new Dictionary<CacheKey, CacheValue>();
            foreach (var number in startingNumbers)
            {
                SecretNumber2(number, cache);
            }
            
            
            return cache.Values.Select(x => x.Value()).Max();
        }

        long SecretNumber(long start)
        {
            var secretNumber = start;
            for (int i = 0; i < 2000; i++)
            {
                var tmp = secretNumber * 64;
                secretNumber = Prune(Mix(secretNumber, tmp));
                tmp = secretNumber / 32;
                secretNumber = Prune(Mix(secretNumber, tmp));
                tmp = secretNumber * 2048;
                secretNumber = Prune(Mix(secretNumber, tmp));
            }

            return secretNumber;
        }
        
        void SecretNumber2(long start, Dictionary<CacheKey, CacheValue> cache)
        {
            var secretNumber = start;
            var prevPrice = (sbyte)(secretNumber % 10);
            var window = new CacheKey();
            for (int i = 0; i < 2000; i++)
            {
                var tmp = secretNumber * 64;
                secretNumber = Prune(Mix(secretNumber, tmp));
                tmp = secretNumber / 32;
                secretNumber = Prune(Mix(secretNumber, tmp));
                tmp = secretNumber * 2048;
                secretNumber = Prune(Mix(secretNumber, tmp));

                var currentPrice = (sbyte)(secretNumber % 10);
                if (i > 0)
                {
                    window = window.Shift((sbyte)(currentPrice - prevPrice));

                    if (i > 2)
                    {
                        var value = cache.GetOrAdd(window, () => new CacheValue());
                        value.Push(start, currentPrice);
                    }
                }

                prevPrice = currentPrice;
            }
        }

        long Mix(long n1, long n2)
        {
            return n1 ^ n2;
        }

        long Prune(long n1)
        {
            return n1 % 16777216;
        }

        readonly record struct CacheKey(sbyte B1, sbyte B2, sbyte B3, sbyte B4)
        {
            public CacheKey Shift(sbyte b)
            {
                return new CacheKey(B2, B3, B4, b);
            }
        }

        record CacheValue()
        {
            private readonly List<long> nodes = new();
            private int value = 0;
            public void Push(long start, int value)
            {
                if (nodes.Any(x => x == start))
                {
                    return;
                }
                
                nodes.Add(start);
                this.value += value;
            }

            public int Value() => value;
        }
    }