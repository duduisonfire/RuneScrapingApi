using Newtonsoft.Json;

namespace RunesWebScraping.domain
{
    public class LolApi : ILolApi
    {
        public async Task<string> GetLolVersion()
        {
            try
            {
                var client = new HttpClient();
                var res = await client.GetAsync(
                    "https://ddragon.leagueoflegends.com/api/versions.json"
                );
                var content = res.Content;
                var dataInString = await content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<List<string>>(dataInString);
                var lolVersion = data != null ? data[0] : "";

                return lolVersion;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return e.Message;
            }
        }

        public async Task<List<string>> GetChampionList()
        {
            try
            {
                var lolVersion = await GetLolVersion();
                var client = new HttpClient();
                var res = await client.GetAsync(
                    $"https://ddragon.leagueoflegends.com/cdn/{lolVersion}/data/en_US/champion.json"
                );
                var content = res.Content;
                var dataInString = await content.ReadAsStringAsync();
                var championsData = JsonConvert.DeserializeObject<ChampionObject>(dataInString);
                var championList = championsData!.data.Keys.ToList();

                return championList;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                var errorList = new List<string> { e.Message };

                return errorList;
            }
        }
    }
}
