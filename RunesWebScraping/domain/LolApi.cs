using Newtonsoft.Json;

namespace RunesWebScraping.domain
{
    public class LolApi
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
                var stringData = await content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<List<string>>(stringData);

                if (data != null)
                {
                    return data[0];
                }

                return "";
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }
    }
}
