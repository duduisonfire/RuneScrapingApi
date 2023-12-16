namespace RunesWebScraping.domain;

public interface ILolApi
{
    Task<string> GetLolVersion();
    Task<List<string>> GetChampionList();
}
