using RunesWebScraping.controllers.interfaces;
using RunesWebScraping.domain;
using RunesWebScraping.infra;

namespace RunesWebScraping.controllers
{
    public class RuneResponse : IRuneResponse
    {
        public string Champion { get; set; }
        public string Lane { get; set; }
        public List<List<string>> Runes { get; set; }
        public List<IRunePage> RunesId { get; set; }

        public RuneResponse(RunesPageBuilder pageBuilder)
        {
            Champion = pageBuilder.champion;
            Lane = pageBuilder.lane;
            Runes = pageBuilder.listOfRunesName.ToList();
            RunesId = pageBuilder.listOfRunesId;
        }
    }
}
