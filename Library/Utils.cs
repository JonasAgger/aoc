using System.Diagnostics;

namespace AdventOfCode.Library;

public static class Utils
{
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