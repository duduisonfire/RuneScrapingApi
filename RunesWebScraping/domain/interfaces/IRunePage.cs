namespace RunesWebScraping.domain.interfaces
{
    public interface IRunePage
    {
        int primaryStyleId { get; }
        int subStyleId { get; }
        List<int> selectedPerksIds { get; }
    }
}
