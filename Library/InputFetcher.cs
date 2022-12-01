using System.Net;

namespace AdventOfCode.Library;

public class InputFetcher
{
    private const string BaseAddress = "https://adventofcode.com";

    private const string Cookie = "53616c7465645f5fa8071a702ec98be896c2bdf2397ed988bca2cd8c8bd2576dabb63a3881bbd5147161190c457732266d805498db9929e81789573cb14b56c6";

    public async Task<string> FetchInput(int year, int day)
    {
        var path = $"/{year}/day/{day}/input";

        var cookieContainer = new CookieContainer();
        cookieContainer.Add(new Cookie("session", Cookie, "/", ".adventofcode.com"));

        using var client = new HttpClient(new HttpClientHandler()
        {
            CookieContainer = cookieContainer
        });

        return await client.GetStringAsync(BaseAddress + path);
    }
}