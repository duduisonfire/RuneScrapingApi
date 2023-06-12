namespace RunesWebScraping.domain.interfaces
{
    public interface IRunePage
    {
        int PrimaryStyleId { get; }
        int SubStyleId { get; }
        List<int> SelectedPerkIds { get; }
    }
}
