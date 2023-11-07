namespace RunesWebScraping.domain.interfaces
{
    public interface IRunePage
    {
        string RunePageTitle { get; set; }
        int PrimaryStyleId { get; }
        int SubStyleId { get; }
        List<int> SelectedPerkIds { get; }
    }
}
