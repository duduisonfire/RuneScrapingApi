using RunesWebScraping.domain;

namespace RunesWebScraping.controllers.interfaces
{
    public interface IRuneResponse
    {
        string Champion { get; set; }
        string Lane { get; set; }
        List<string> Runes { get; set; }
        RunePage RunesId { get; set; }
    }
}
