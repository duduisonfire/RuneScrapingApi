using AngleSharp;

namespace RunesWebScraping.cases;

public class UggWebScrap
{
    private readonly string _Champion;
    private readonly string _Lane;
    private List<string> _Pages = new();

    public UggWebScrap(string champion, string lane)
    {
        _Champion = champion;
        _Lane = lane;
    }

    public async Task<List<List<string>>> GetRunes()
    {
        await ScrapPrincipalRunePage();
        var builds = await GetBuildsNames();
        await ScrapOthersPage(builds);

        List<List<string>> runesPack = new();

        for (int i = 0; i < _Pages.Count; i++)
        {
            var runes = await SelectRunesFromPage(i);
            runes.Add(builds[i]);
            runesPack.Add(runes);
        }

        return runesPack;
    }

    public static async Task<string> GetChampionLane(string champ)
    {
        try
        {
            var url = $"https://u.gg/lol/champions/{champ}/build/";
            var httpClient = new HttpClient();
            var myRequest = await httpClient.GetAsync(url);
            myRequest.EnsureSuccessStatusCode();
            var content = await myRequest.Content.ReadAsStringAsync();

            var context = BrowsingContext.New(Configuration.Default);
            var document = await context.OpenAsync(req => req.Content(content));

            var roleValue = document.QuerySelector(".role-value")!.ChildNodes.ToList();
            var role = roleValue[1].TextContent;

            return role;
        }
        catch (Exception)
        {
            return await GetChampionLane(champ);
        }
    }

    private async Task<List<string>> SelectRunesFromPage(int page)
    {
        try
        {
            var context = BrowsingContext.New(Configuration.Default);
            var document = await context.OpenAsync(req => req.Content(_Pages[page]));

            var treeNameList = document
                .QuerySelectorAll(".perk-style-title")
                .Select(e => e.InnerHtml)
                .ToList();

            var majorRuneList = document
                .QuerySelectorAll(".perk-active")
                .Select(e => e.FirstElementChild!.GetAttribute("alt")!)
                .ToList();

            var minorsRuneList = document
                .QuerySelectorAll(".shard-active")
                .Select(e => e.FirstElementChild!.GetAttribute("alt")!)
                .ToList();

            List<string> runes = new();

            treeNameList.RemoveRange(2, 2);
            majorRuneList.RemoveRange(6, 6);
            minorsRuneList.RemoveRange(3, 3);

            runes.AddRange(treeNameList);
            runes.AddRange(majorRuneList);
            runes.AddRange(minorsRuneList);

            return runes;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw new Exception(
                "You didn't pass the correct parameters. Please choose a champion and a valid lane."
            );
        }
    }

    private async Task<List<string>> GetBuildsNames()
    {
        var context = BrowsingContext.New(Configuration.Default);
        var document = await context.OpenAsync(req => req.Content(_Pages[0]));

        var buildValue = document.QuerySelectorAll(".build-name")!.ToList();
        List<string> build = new();

        foreach (var item in buildValue)
        {
            build.Add(item.TextContent);
        }

        return build;
    }

    private async Task ScrapOthersPage(List<string> builds)
    {
        for (int i = 1; i < builds.Count; i++)
        {
            try
            {
                var url =
                    $"https://u.gg/lol/champions/{builds[i].ToLower()}/{_Champion}/build/{_Lane}?rank=overall";
                var httpClient = new HttpClient();
                var myRequest = await httpClient.GetAsync(url);
                myRequest.EnsureSuccessStatusCode();
                var content = await myRequest.Content.ReadAsStringAsync();
                _Pages.Add(content);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }

    private async Task ScrapPrincipalRunePage()
    {
        try
        {
            var url = $"https://u.gg/lol/champions/{_Champion}/build/{_Lane}?rank=overall";
            var httpClient = new HttpClient();
            var myRequest = await httpClient.GetAsync(url);
            myRequest.EnsureSuccessStatusCode();
            var content = await myRequest.Content.ReadAsStringAsync();
            _Pages.Add(content);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}
