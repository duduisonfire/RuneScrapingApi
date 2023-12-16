using RunesWebScraping.controllers.interfaces;
using RunesWebScraping.models;

namespace RunesWebScraping.repository;

public interface IUggRepository
{
    Task<UggDB> CreateChampionCache(IRuneResponse response);
    Task<UggDB?> ChampionCacheExists(string champion, string lane);
    Task<UggDB> UpdateChampionCache(IRuneResponse response);
    Task<long> ChampionRunesCacheLength();
}
