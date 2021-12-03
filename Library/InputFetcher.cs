using System.Net;

namespace AdventOfCode.Library;

public class InputFetcher
{
    private const string BaseAddress = "https://adventofcode.com";

    private const string Cookie = "";

    public async Task<string> FetchInput(int day)
    {
        var path = $"/2021/day/{day}/input";

        var cookieContainer = new CookieContainer();
        cookieContainer.Add(new Cookie("session", "53616c7465645f5f2069f304f9c6d340243c5d6090d9534d29092cf0021cfc1e50847bbc2a524feed4b54f8e93361077", path, "adventofcode.com"));

        using var client = new HttpClient(new HttpClientHandler()
        {
            CookieContainer = cookieContainer
        });

        return await client.GetStringAsync(BaseAddress + path);
    }
}