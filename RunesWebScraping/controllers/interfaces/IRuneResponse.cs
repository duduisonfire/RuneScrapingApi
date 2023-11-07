using RunesWebScraping.domain;

namespace RunesWebScraping.controllers.interfaces
{
    public interface IRuneResponse
    {
        string Champion { get; set; }
        string Lane { get; set; }
        List<List<string>> Runes { get; set; }
        List<RunePage> RunesId { get; set; }
    }
}
