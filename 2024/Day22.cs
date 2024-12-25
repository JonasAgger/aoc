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
            var cache = new Dictionary<PriceWindowCacheKey, PriceCache>();
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
                secretNumber = SecretNumberIteration(secretNumber);
            }

            return secretNumber;
        }
        
        void SecretNumber2(long start, Dictionary<PriceWindowCacheKey, PriceCache> cache)
        {
            var secretNumber = start;
            var prevPrice = (sbyte)(secretNumber % 10);
            var window = new PriceWindowCacheKey();
            for (int i = 0; i < 2000; i++)
            {
                secretNumber = SecretNumberIteration(secretNumber);
                var currentPrice = (sbyte)(secretNumber % 10);
                window = window.Shift((sbyte)(currentPrice - prevPrice)); // Basically just a dummy class so that I don't have to bitshift a int.
                
                // If we have a full window, then add it to the cache
                if (i > 2)
                {
                    // Every pricecache keeps track of the starting number, and the current price for the given current sequence
                    var value = cache.GetOrAdd(window, () => new PriceCache());
                    value.Push(start, currentPrice);
                }

                prevPrice = currentPrice;
            }
        }

        private static long SecretNumberIteration(long secretNumber)
        {
            var tmp = secretNumber * 64;
            secretNumber = Prune(Mix(secretNumber, tmp));
            tmp = secretNumber / 32;
            secretNumber = Prune(Mix(secretNumber, tmp));
            tmp = secretNumber * 2048;
            return Prune(Mix(secretNumber, tmp));
        }

        private static long Mix(long n1, long n2)
        {
            return n1 ^ n2;
        }

        private static long Prune(long n1)
        {
            return n1 % 16777216;
        }

        private readonly record struct PriceWindowCacheKey(sbyte B1, sbyte B2, sbyte B3, sbyte B4)
        {
            public PriceWindowCacheKey Shift(sbyte b)
            {
                return new PriceWindowCacheKey(B2, B3, B4, b);
            }
        }

        private record PriceCache()
        {
            private readonly List<long> nodes = [];
            private int cumPrice = 0;
            public void Push(long start, int price)
            {
                if (nodes.Any(x => x == start))
                {
                    return;
                }
                
                nodes.Add(start);
                this.cumPrice += price;
            }

            public int Value() => cumPrice;
        }
    }