using RunesWebScraping.domain;
using RunesWebScraping.repository;

namespace RunesWebScraping.cases;

public class CacheManager
{
    private readonly RuneCacheSync _runeCacheSync;
    private readonly UggRepository _uggRepository;
    private readonly ChampionsRepository _championsRepository;
    private readonly LolApi _lolApi;
    private readonly ChampionsListCacheSync _championsListCacheSync;

    public CacheManager(
        UggRepository uggRepository,
        RuneCacheSync runeCacheSync,
        ChampionsRepository championsRepository,
        ChampionsListCacheSync championsListCacheSync,
        LolApi lolApi
    )
    {
        _uggRepository = uggRepository;
        _runeCacheSync = runeCacheSync;
        _lolApi = lolApi;
        _championsRepository = championsRepository;
        _championsListCacheSync = championsListCacheSync;
    }

    public async Task SyncRunesCache()
    {
        try
        {
            await _runeCacheSync.UpdateAllRunes();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    public async Task SyncChampionsCache()
    {
        try
        {
            await _championsListCacheSync.UpdateAllChampions();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    public async Task<bool> VerifyRunesCache()
    {
        try
        {
            var championListLength = (await _lolApi.GetChampionList()).Count;
            var haveCache =
                await _uggRepository.ChampionRunesCacheLength() / 5 == championListLength;

            return haveCache;
        }
        catch (Exception e)
        {
            Console.WriteLine($"{e.Message}");
            throw;
        }
    }

    public async Task<bool> VerifyChampionsLaneCache()
    {
        try
        {
            var championListLength = (await _lolApi.GetChampionList()).Count;
            var haveCache =
                await _championsRepository.ChampionsLaneCacheLength() == championListLength;

            return haveCache;
        }
        catch (Exception e)
        {
            Console.WriteLine($"{e.Message}");
            throw;
        }
    }

    public async Task UpdateAllCaches()
    {
        var hasRunesCache = await VerifyRunesCache();
        var hasChampionsCache = await VerifyChampionsLaneCache();

        if (!hasRunesCache)
        {
            await SyncRunesCache();
            Console.WriteLine("Syncing the Runes cache");
        }
        else
        {
            Console.WriteLine("Have Runes cache");
        }

        if (!hasChampionsCache)
        {
            await SyncChampionsCache();
            Console.WriteLine("Syncing the champions cache");
        }
        else
        {
            Console.WriteLine("Have Champions cache");
        }
    }
}
