using System.Net;

namespace AdventOfCode;

public class InputFetcher
{
    private const string AoCFolder = "AdventOfCodeStorage";
    private const string CookieFile = "personal.cookie";
    private const string BaseAddress = "https://adventofcode.com";
    private static string cookie;
    public async ValueTask<string> FetchInput(int year, int day)
    {
        await FetchCookie();
        
        var path = $"/{year}/day/{day}/input";

        var cookieContainer = new CookieContainer();
        cookieContainer.Add(new Cookie("session", cookie, path, "adventofcode.com"));

        using var client = new HttpClient(new HttpClientHandler()
        {
            CookieContainer = cookieContainer
        });

        return await client.GetStringAsync(BaseAddress + path);
    }
    
    public async ValueTask<string> FetchTestInput(int year, int day)
    {
        await FetchCookie();
        
        var path = $"/{year}/day/{day}";

        var cookieContainer = new CookieContainer();
        cookieContainer.Add(new Cookie("session", cookie, path, "adventofcode.com"));

        using var client = new HttpClient(new HttpClientHandler()
        {
            CookieContainer = cookieContainer
        });

        return await client.GetStringAsync(BaseAddress + path);
    }


    private async ValueTask FetchCookie()
    {
        if (!string.IsNullOrEmpty(cookie)) return;

        var cookieDirectory = Path.Combine(Path.GetTempPath(), AoCFolder);
        var cookiePath = Path.Combine(cookieDirectory, CookieFile);
        // File.Delete(cookiePath);
        if (!File.Exists(cookiePath))
        {
            Console.WriteLine("Could not find personal cookie or the previously entered cookie dident work.");
            Console.WriteLine(" Please enter the cookie when searching for input on " + BaseAddress);
            Console.Write("cookie: ");
            cookie = Console.ReadLine();

            if (!Directory.Exists(cookieDirectory)) Directory.CreateDirectory(cookieDirectory);
            
            await File.WriteAllTextAsync(cookiePath, cookie);
        }
        else
        {
            cookie = await File.ReadAllTextAsync(cookiePath);
        }
    }
}