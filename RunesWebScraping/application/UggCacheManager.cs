using RunesWebScraping.domain;
using RunesWebScraping.services;

namespace RunesWebScraping;

public class UggCacheManager
{
    public bool hasCache = false;
    private readonly UggRuneCacheSync _runeCacheSync;
    private readonly UggService _uggService;
    private readonly LolApi _lolApi;

    public UggCacheManager(UggService uggService, UggRuneCacheSync runeCacheSync, LolApi lolApi)
    {
        _uggService = uggService;
        _runeCacheSync = runeCacheSync;
        _lolApi = lolApi;
    }

    public async Task SyncCache()
    {
        try
        {
            await _runeCacheSync.UpdateAllRunes();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return;
        }
    }

    public async Task<bool> VerifyCache()
    {
        try
        {
            var championListLength = (await _lolApi.GetChampionList()).Count;
            var haveCache = await _uggService.ChampionRunesCacheLength() / 5 == championListLength;

            return haveCache;
        }
        catch (Exception e)
        {
            Console.WriteLine($"{e.Message}");
            throw;
        }
    }
}
