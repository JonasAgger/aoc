using System.Buffers.Binary;
using System.Numerics;
using AdventOfCode.Library;

    namespace AdventOfCode._2022;

    public class Day11 : DayEngine
    {
        public override string[] TestInput => new string[]
        {
            "Monkey 0:",
            "  Starting items: 79, 98",
            "  Operation: new = old * 19",
            "  Test: divisible by 23",
            "    If true: throw to monkey 2",
            "    If false: throw to monkey 3",
            "Monkey 1:",
            "  Starting items: 54, 65, 75, 74",
            "  Operation: new = old + 6",
            "  Test: divisible by 19",
            "    If true: throw to monkey 2",
            "    If false: throw to monkey 0",
            "Monkey 2:",
            "  Starting items: 79, 60, 97",
            "  Operation: new = old * old",
            "  Test: divisible by 13",
            "    If true: throw to monkey 1",
            "    If false: throw to monkey 3",
            "Monkey 3:",
            "  Starting items: 74",
            "  Operation: new = old + 3",
            "  Test: divisible by 17",
            "    If true: throw to monkey 0",
            "    If false: throw to monkey 1",
        };

        protected override object HandlePart1(string[] input)
        {
            var monkeys = input.Chunk(6).Select(ParseMoney).ToList();
            var rounds = 20;

            for (int i = 0; i < rounds; i++)
            {
                foreach (var monkey in monkeys)
                {
                    while (monkey.Items.TryDequeue(out var item))
                    {
                        monkey.Count += 1;
                        item = monkey.Operation(item);
                        item /= 3;
                    
                        if (item % monkey.Test == 0)
                        {
                            monkeys[monkey.True].Items.Enqueue(item);
                        }
                        else
                        {
                            monkeys[monkey.False].Items.Enqueue(item);
                        }
                    }
                }
            }
            
            return monkeys.OrderByDescending(x => x.Count).Take(2).Aggregate(1l, (i, monkey) => i * monkey.Count);
        }

        protected override object HandlePart2(string[] input)
        {
            var monkeys = input.Chunk(6).Select(ParseMoney).ToList();

            var modulusTrick = monkeys.Aggregate(1L, (i, monkey) => i * monkey.Test); // Finding the common modulus value for all monkeys
            
            for (int i = 0; i < 10_000; i++)
            {
                foreach (var monkey in monkeys)
                {
                    while (monkey.Items.TryDequeue(out var item))
                    {
                        monkey.Count += 1;
                        item = monkey.Operation(item) % modulusTrick;

                        if (item % monkey.Test == 0)
                        {
                            monkeys[monkey.True].Items.Enqueue(item);
                        }
                        else
                        {
                            monkeys[monkey.False].Items.Enqueue(item);
                        }
                    }
                }
            }
            
            return monkeys.OrderByDescending(x => x.Count).Take(2).Aggregate(1l, (i, monkey) => i * monkey.Count);
        }

        private Monkey ParseMoney(string[] input)
        {
            input = input.Skip(1).ToArray();
            
            var items = input[0]
                .Trim()
                .Replace("Starting items: ", "")
                .Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Long())
                .ToQueue();

            var operationParts = input[1]
                .Trim()
                .Replace("Operation: new = ", "")
                .Split(' ');

            var op = (long x) =>
            {
                var i1 = operationParts[0] switch
                {
                    "old" => x,
                    (string val) => val.Long()
                };

                var i2 = operationParts[2] switch
                {
                    "old" => x,
                    (string val) => val.Long()
                };

                return operationParts[1] switch
                {
                    "+" => i1 + i2,
                    "*" => i1 * i2,
                };
            };
            
            var testInt = input[2]
                .Trim()
                .Replace("Test: divisible by ", "")
                .Int();

            var trueBranch = input[3].Split().Last().Int();
            var falseBranch = input[4].Split().Last().Int();

            return new Monkey()
            {
                Items = items,
                Operation = op,
                Test = testInt,
                True = trueBranch,
                False = falseBranch
            };
        }
        
        private class Monkey
        {
            public int Count = 0;
            
            public Queue<long> Items;

            public Func<long, long> Operation;
            public long Test { get; set; }
            
            public int True { get; set; }
            public int False { get; set; }
        }
    }