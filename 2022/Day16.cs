using AdventOfCode.Library;

namespace AdventOfCode._2022;

public class Day16 : DayEngine
{
    public override string[] TestInput => new[]
    {
        "Valve AA has flow rate=0; tunnels lead to valves DD, II, BB",
        "Valve BB has flow rate=13; tunnels lead to valves CC, AA",
        "Valve CC has flow rate=2; tunnels lead to valves DD, BB",
        "Valve DD has flow rate=20; tunnels lead to valves CC, AA, EE",
        "Valve EE has flow rate=3; tunnels lead to valves FF, DD",
        "Valve FF has flow rate=0; tunnels lead to valves EE, GG",
        "Valve GG has flow rate=0; tunnels lead to valves FF, HH",
        "Valve HH has flow rate=22; tunnel leads to valve GG",
        "Valve II has flow rate=0; tunnels lead to valves AA, JJ",
        "Valve JJ has flow rate=21; tunnel leads to valve II"
    };


    protected override object HandlePart1(string[] input)
    {
        var map = Parse(input);
        return Solve(map, true, 30);
    }

    protected override object HandlePart2(string[] input)
    {
        var map = Parse(input);
        return Solve(map, false, 26);
    }

    private record Map(int[,] Distances, Valve[] Valves);

    private record Valve(int id, string name, int flowRate, string[] tunnels);

    private int Solve(Map map, bool humanOnly, int time)
    {
        var start = map.Valves.Single(x => x.name == "AA");
        var valvesToOpen = map.Valves.Where(valve => valve.flowRate > 0).ToArray();

        var cache = new Dictionary<string, int>();
        if (humanOnly)
            return MaxFlow(cache, map, start, valvesToOpen.ToHashSet(), time);
        return Pairings(valvesToOpen).Select(pairing =>
            MaxFlow(cache, map, start, pairing.human, time) +
            MaxFlow(cache, map, start, pairing.elephant, time)
        ).Max();
    }

    // Divide the valves between human and elephant in all possible ways
    private IEnumerable<(HashSet<Valve> human, HashSet<Valve> elephant)> Pairings(Valve[] valves)
    {
        var maxMask = 1 << (valves.Length - 1);

        for (var mask = 0; mask < maxMask; mask++)
        {
            var elephant = new HashSet<Valve>();
            var human = new HashSet<Valve>();

            elephant.Add(valves[0]);

            for (var ivalve = 1; ivalve < valves.Length; ivalve++)
                if ((mask & (1 << ivalve)) == 0)
                    human.Add(valves[ivalve]);
                else
                    elephant.Add(valves[ivalve]);
            yield return (human, elephant);
        }
    }

    private int MaxFlow(
        Dictionary<string, int> cache,
        Map map,
        Valve currentValve,
        HashSet<Valve> valves,
        int remainingTime
    )
    {
        var key =
            remainingTime + "-" +
            currentValve.id + "-" +
            string.Join("-", valves.OrderBy(x => x.id).Select(x => x.id));

        if (!cache.ContainsKey(key))
        {
            // current valve gives us this much flow:
            var flowFromValve = currentValve.flowRate * remainingTime;

            // determine best use of the remaining time:
            var flowFromRest = 0;
            foreach (var valve in valves.ToArray())
            {
                var distance = map.Distances[currentValve.id, valve.id];

                if (remainingTime >= distance + 1)
                {
                    valves.Remove(valve);
                    remainingTime -= distance + 1;

                    flowFromRest = Math.Max(flowFromRest, MaxFlow(cache, map, valve, valves, remainingTime));

                    remainingTime += distance + 1;
                    valves.Add(valve);
                }
            }

            cache[key] = flowFromValve + flowFromRest;
        }

        return cache[key];
    }

    private Map Parse(string[] input)
    {
        // Valve BB has flow rate=0; tunnels lead to valve CC
        // Valve CC has flow rate=10; tunnels lead to valves DD, EE
        var valveList = new List<Valve>();
        foreach (var line in input)
        {
            var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            var name = parts[1];
            var flow = parts[4].Split('=').Last()[..^1].Int();
            var tunnels = parts.Skip(9).Select(x => x.Replace(",", "").Trim()).ToArray();
            valveList.Add(new Valve(0, name, flow, tunnels));
        }

        var valves = valveList
            .OrderByDescending(valve => valve.flowRate)
            .Select((v, i) => v with { id = i })
            .ToArray();

        return new Map(ComputeDistances(valves), valves);
    }

    private int[,] ComputeDistances(Valve[] valves)
    {
        // Bellman-Ford style distance calculation for every pair of valves
        var distances = new int[valves.Length, valves.Length];
        for (var i = 0; i < valves.Length; i++)
        for (var j = 0; j < valves.Length; j++)
            distances[i, j] = int.MaxValue;
        foreach (var valve in valves)
        foreach (var target in valve.tunnels)
        {
            var targetNode = valves.Single(x => x.name == target);
            distances[valve.id, targetNode.id] = 1;
            distances[targetNode.id, valve.id] = 1;
        }

        var n = distances.GetLength(0);
        var done = false;
        while (!done)
        {
            done = true;
            for (var source = 0; source < n; source++)
            for (var target = 0; target < n; target++)
                if (source != target)
                    for (var through = 0; through < n; through++)
                    {
                        if (distances[source, through] == int.MaxValue || distances[through, target] == int.MaxValue)
                            continue;
                        var cost = distances[source, through] + distances[through, target];
                        if (cost < distances[source, target])
                        {
                            done = false;
                            distances[source, target] = cost;
                            distances[target, source] = cost;
                        }
                    }
        }

        return distances;
    }
}