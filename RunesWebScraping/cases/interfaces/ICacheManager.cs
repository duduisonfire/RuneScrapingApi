namespace RunesWebScraping.cases;

public interface ICacheManager {
    Task SyncRunesCache();
    Task SyncChampionsCache();
    Task<bool> VerifyRunesCache();
    Task<bool> VerifyChampionsLaneCache();
    Task UpdateAllCaches();
}