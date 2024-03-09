using RunesWebScraping.models;

namespace RunesWebScraping.repository;

public interface IChampionsRepository
{
    Task<ChampionsLane> CreateChampionLaneCache(string champion, string lane);
    Task<ChampionsLane?> GetChampionLane(string champion);
    Task<long> ChampionsLaneCacheLength();
    Task<List<ChampionsLane>> GetAllChampions();
}
