using RunesWebScraping.domain;
using RunesWebScraping.models;
using RunesWebScraping.repository;

namespace RunesWebScraping.cases;

public class ChampionsListCacheSync
{
    private readonly ChampionsRepository _championsRepository;
    private readonly LolApi _lolApi;

    public ChampionsListCacheSync(ChampionsRepository championsRepository, LolApi lolApi)
    {
        _championsRepository = championsRepository;
        _lolApi = lolApi;
    }

    public async Task<ChampionsLane> UpdateChampionCache(string champion)
    {
        while (true)
        {
            try
            {
                var lane = await UggWebScrap.GetChampionLane(champion);

                var championCache = await _championsRepository.CreateChampionLaneCache(
                    champion,
                    lane
                );
                Console.WriteLine($"The Champions Cache of {champion} are created.");

                return championCache;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }

    public async Task UpdateAllChampions()
    {
        try
        {
            var championList = await _lolApi.GetChampionList();

            for (int i = 0; i < championList.Count; i++)
            {
                await UpdateChampionCache(championList[i]);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return;
        }
    }

    public async Task<string> GetChampionLane(string champion)
    {
        try
        {
            var championLane = await _championsRepository.GetChampionLane(champion);

            if (championLane == null)
                return "not found";

            return championLane!.Lane;
        }
        catch (Exception)
        {
            throw;
        }
    }
}
