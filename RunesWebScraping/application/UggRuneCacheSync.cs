using RunesWebScraping.application;
using RunesWebScraping.controllers.classes;
using RunesWebScraping.domain;
using RunesWebScraping.services;

namespace RunesWebScraping;

public class UggRuneCacheSync
{
    private readonly UggService _uggDbService;
    private readonly LolApi _lolApi;

    public UggRuneCacheSync(UggService uggService, LolApi lolApi)
    {
        _uggDbService = uggService;
        _lolApi = lolApi;
    }

    public async Task<RuneResponse> UpdateChampionCache(string champion, string lane)
    {
        while (true)
        {
            try
            {
                var webScrap = new RuneWebScrap(champion, lane);
                await webScrap.GetRunes();
                var runesId = new RunePage(webScrap.runeList);
                var runeResponse = new RuneResponse(webScrap, runesId);

                var ChampionCacheExists = await _uggDbService.ChampionCacheExists(champion, lane);

                if (ChampionCacheExists != null)
                {
                    await _uggDbService.UpdateChampionCache(runeResponse);
                    Console.WriteLine($"The runes of {champion} in {lane} are updated.");
                }
                else
                {
                    await _uggDbService.CreateChampionCache(runeResponse);
                    Console.WriteLine($"The runes of {champion} in {lane} are created.");
                }

                return runeResponse;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }

    public async Task UpdateAllRunes()
    {
        try
        {
            var championList = await _lolApi.GetChampionList();

            for (int i = 0; i < championList.Count; i++)
            {
                await UpdateChampionCache(championList[i], "mid");
                await UpdateChampionCache(championList[i], "top");
                await UpdateChampionCache(championList[i], "jungle");
                await UpdateChampionCache(championList[i], "adc");
                await UpdateChampionCache(championList[i], "supp");

                Console.WriteLine($"Updated {championList[i]} runes cache.");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return;
        }
    }
}
