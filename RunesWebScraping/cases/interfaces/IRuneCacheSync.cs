using RunesWebScraping.controllers.interfaces;

namespace RunesWebScraping.cases;

public interface IRuneCacheSync
{
    Task<IRuneResponse> UpdateChampionCache(string champion, string lane);
    Task UpdateAllRunes();
}
