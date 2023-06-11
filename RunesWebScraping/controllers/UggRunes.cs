using Microsoft.AspNetCore.Mvc;
using RunesWebScraping.application;
using RunesWebScraping.domain;

namespace RunesWebScraping.controllers
{
    [ApiController]
    [Route("api/ugg")]
    public class UggRunes : Controller
    {
        [HttpGet]
        public async Task<HashSet<string>> GetRunes()
        {
            var webScrap = new RuneWebScrap("ahri", "mid");
            await webScrap.GetRunes();
            return webScrap.runeList;
        }
    }
}
