using RunesWebScraping.models;

namespace RunesWebScraping.cases;

public interface IChampionsListCacheSync
{
    Task<ChampionsLane> UpdateChampionCache(string champion);
    Task UpdateAllChampions();
    Task<string> GetChampionLane(string champion);
}
