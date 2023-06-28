using RunesWebScraping;
using RunesWebScraping.domain;
using RunesWebScraping.services;

namespace Tests;

public class UnitTest1
{
    [Fact]
    public async Task Test1()
    {
        Environment.SetEnvironmentVariable("CONNECTIONSTRING", "mongodb+srv://igortiburciocs:Zerocoll12@cluster0.uh1myey.mongodb.net/");
        var cacheSync = new RuneCacheSync(new UggService(), new LolApi());
        var runeResponse = await cacheSync.UpdateChampionCache("fizz", "top");

        Assert.True(runeResponse.RunesId.SelectedPerkIds.Count == 9);
    }
}
