using RunesWebScraping.domain;
using RunesWebScraping.repository;

namespace RunesWebScraping.cases;

public class CacheManager : ICacheManager
{
    private readonly IRuneCacheSync _runeCacheSync;
    private readonly IUggRepository _uggRepository;
    private readonly IChampionsRepository _championsRepository;
    private readonly ILolApi _lolApi;
    private readonly IChampionsListCacheSync _championsListCacheSync;

    public CacheManager(
        IUggRepository uggRepository,
        IRuneCacheSync runeCacheSync,
        IChampionsRepository championsRepository,
        IChampionsListCacheSync championsListCacheSync,
        ILolApi lolApi
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
                (await _uggRepository.ChampionRunesCacheLength() / 5) == championListLength;

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
