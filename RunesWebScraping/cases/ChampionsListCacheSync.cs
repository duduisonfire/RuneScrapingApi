using RunesWebScraping.domain;
using RunesWebScraping.infra;
using RunesWebScraping.models;
using RunesWebScraping.repository;

namespace RunesWebScraping.cases;

public class ChampionsListCacheSync : IChampionsListCacheSync
{
    private readonly IChampionsRepository _championsRepository;
    private readonly ILolApi _lolApi;

    public ChampionsListCacheSync(IChampionsRepository championsRepository, ILolApi lolApi)
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
                var laneSanitizer = new LaneSanitizer(lane);

                var championCache = await _championsRepository.CreateChampionLaneCache(
                    champion,
                    laneSanitizer.NormalizedLaneName
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
