using RunesWebScraping.application;
using RunesWebScraping.controllers.classes;
using RunesWebScraping.domain;
using RunesWebScraping.services;

namespace RunesWebScraping;

public class RuneCacheSync
{
    private readonly UggService _uggDbService;

    public RuneCacheSync(UggService uggService)
    {
        _uggDbService = uggService;
    }

    private async Task UpdateChampionCache(string champion, string lane)
    {
        try
        {
            var webScrap = new RuneWebScrap(champion, lane);
            await webScrap.GetRunes();
            var runesId = new RunePage(webScrap.runeList.ToList());
            var runeResponse = new RuneResponse(webScrap, runesId);

            var ChampionCacheExists = await _uggDbService.ChampionCacheExists(champion, lane);

            if (ChampionCacheExists != null)
            {
                await _uggDbService.UpdateChampionCache(runeResponse);
                Console.WriteLine($"The runes of {champion} in {lane} are updated.");
                return;
            }

            await _uggDbService.CreateChampionCache(runeResponse);
            Console.WriteLine($"The runes of {champion} in {lane} are created.");
        } catch (Exception)
        {
            Console.WriteLine("The u.gg website is experiencing problems and we can't get runes data.");
            throw;
        }
    }

    public async Task UpdateAllRunes()
    {
        var lolApi = new LolApi();
        var championList = await lolApi.GetChampionList();

        for (int i = 0; i < championList.Count; i++)
        {
            await UpdateChampionCache(championList[i], "mid");
            await UpdateChampionCache(championList[i], "top");
            await UpdateChampionCache(championList[i], "jungle");
            await UpdateChampionCache(championList[i], "adc");
            await UpdateChampionCache(championList[i], "support");

            Console.WriteLine($"Updated {championList[i]} runes cache.");
        }
    }
}
