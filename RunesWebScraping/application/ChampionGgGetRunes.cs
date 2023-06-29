namespace RunesWebScraping;

public class ChampionGgGetRunes
{
    public readonly string _champion;
    public readonly string _lane;
    public readonly string _champId;

    public ChampionGgGetRunes(string champion, string lane)
    {
        _champion = champion;
        _lane = lane;
    }

    private async Task<string> GetChampionGgRunes()
    {
        try
        {
            var url =
                @$"https://league-champion-aggregate.iesdev.com/
                graphql?query=query%20ChampionBuilds%28%24championId
                %3AInt%21%2C%24queue%3AQueue%21%2C%24role%3ARole
                %2C%24opponentChampionId%3AInt%2C%24key%3AChampionBuildKey
                %29%7BchampionBuildStats%28championId%3A%24championId%2Cqueue
                %3A%24queue%2Crole%3A%24role%2CopponentChampionId%3A%24opponentChampionId
                %2Ckey%3A%24key%29%7BchampionId%20opponentChampionId%20queue%20role
                %20builds%7BcompletedItems%7Bgames%20index%20averageIndex%20itemId
                %20wins%7Dgames%20mythicId%20mythicAverageIndex%20primaryRune%20runes
                %7Bgames%20index%20runeId%20wins%20treeId%7DskillOrders%7Bgames%20skillOrder
                %20wins%7DstartingItems%7Bgames%20startingItemIds%20wins%7DsummonerSpells
                %7Bgames%20summonerSpellIds%20wins%7Dwins%7D%7D%7D&variables=%7B%22championId
                %22%3A{_champId}%2C%22role%22%3A%22{_lane}%22%2C%22queue%22%3A%22RANKED_SOLO_5X5
                %22%2C%22opponentChampionId%22%3Anull%2C%22key%22%3A%22PUBLIC%22%7D";

            var httpClient = new HttpClient();
            var myRequest = await httpClient.GetAsync(url);
            myRequest.EnsureSuccessStatusCode();
            var content = await myRequest.Content.ReadAsStringAsync();

            return content;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return e.Message;
        }
    }

    private async Task ParseJsonRunes()
    {
        var jsonRunesString = await GetChampionGgRunes(); 

    }
}
