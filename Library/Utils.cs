using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace AdventOfCode.Library;

public static class Utils
{
    public static int Int(this string str) => int.Parse(str);
    public static long Long(this string str) => long.Parse(str);

    public static int IndexOf<T>(this T[] stuff, Func<T, bool> predicate)
    {
        for (int i = 0; i < stuff.Length; i++)
        {
            if (predicate(stuff[i])) return i;
        }

        return -1;
    }

    public static int Mod(int input, int mod)
    {
        var returnVal = input % mod;
        return returnVal > 0 ? returnVal : input;
    }

    public static T GetOrAdd<T, K>(this Dictionary<K, T> dictionary, K key, Func<T> addFunc)
    {
        if (dictionary.TryGetValue(key, out var val)) return val;
        val = addFunc();
        dictionary[key] = val;
        return val;
    }

    public static bool TryGet<T>(this List<T> lst, int index, [NotNullWhen(true)] out T? val)
    {
        if (lst.Count > index)
        {
            val = lst[index]!;
            return true;
        }

        val = default;
        return false;
    }
    
    public static Queue<T> ToQueue<T>(this IEnumerable<T> e)
    {
        var q = new Queue<T>();
        foreach(var element in e)
            q.Enqueue(element);
        return q;
    }

    public static LinkedList<T> ToLinkedList<T>(this IEnumerable<T> e)
    {
        var ll = new LinkedList<T>();
        foreach (var element in e)
            ll.AddLast(element);
        return ll;
    }

    public static string GetTimeString(double elapsed)
    {
        var resolution = ToNanoSeconds(elapsed);
        if (resolution < 10000d)
        {
            return $"{resolution:F3} ns";
        }
        
        resolution = ToMicroSeconds(elapsed);
        if (resolution < 10000d)
        {
            return $"{resolution:F3} us";
        }
        
        resolution = ToMiliSeconds(elapsed);
        return $"{resolution:F4} ms";
    }

    public static double ToMiliSeconds(double elapsed)
    {
        return (elapsed * 1000) / Stopwatch.Frequency;
    }
    public static double ToMicroSeconds(double elapsed)
    {
        return (elapsed * 1000000) / Stopwatch.Frequency;
    }
    public static double ToNanoSeconds(double elapsed)
    {
        return (elapsed * 1000000000) / Stopwatch.Frequency;
    }
}