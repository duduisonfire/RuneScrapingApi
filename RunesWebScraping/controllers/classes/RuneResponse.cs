using RunesWebScraping.application;
using RunesWebScraping.controllers.interfaces;
using RunesWebScraping.domain;

namespace RunesWebScraping.controllers.classes
{
    public class RuneResponse : IRuneResponse
    {
        public string Champion { get; set; }
        public string Lane { get; set; }
        public List<string> Runes { get; set; }
        public RunePage RunesId { get; set; }

        public RuneResponse(RuneWebScrap webScrap, RunePage runePage)
        {
            Champion = webScrap.champion;
            Lane = webScrap.lane;
            Runes = webScrap.runeList.ToList();
            RunesId = runePage;
        }
    }
}
