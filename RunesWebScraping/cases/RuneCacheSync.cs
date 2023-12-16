using RunesWebScraping.controllers.classes;
using RunesWebScraping.controllers.interfaces;
using RunesWebScraping.domain;
using RunesWebScraping.infra;
using RunesWebScraping.repository;

namespace RunesWebScraping.cases;

public class RuneCacheSync : IRuneCacheSync
{
    private readonly IUggRepository _uggDbRepository;
    private readonly ILolApi _lolApi;

    public RuneCacheSync(IUggRepository uggRepository, ILolApi lolApi)
    {
        _uggDbRepository = uggRepository;
        _lolApi = lolApi;
    }

    public async Task<IRuneResponse> UpdateChampionCache(string champion, string lane)
    {
        while (true)
        {
            try
            {
                var webScrap = new UggWebScrap(champion, lane);
                var runes = await webScrap.GetRunes();
                var pageBuilder = new RunesPageBuilder(runes, champion, lane);
                var runesId = new List<RunePage>();

                for (int i = 0; i < pageBuilder.listOfRunesId.Count; i++)
                {
                    runesId.Add(pageBuilder.listOfRunesId[i]);
                }

                var runeResponse = new RuneResponse(pageBuilder);

                var ChampionCacheExists = await _uggDbRepository.ChampionCacheExists(
                    champion,
                    lane
                );

                if (ChampionCacheExists != null)
                {
                    await _uggDbRepository.UpdateChampionCache(runeResponse);
                    Console.WriteLine($"The runes of {champion} in {lane} are updated.");
                }
                else
                {
                    await _uggDbRepository.CreateChampionCache(runeResponse);
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
