using RunesWebScraping.application;
using RunesWebScraping.controllers.classes;
using RunesWebScraping.domain;
using RunesWebScraping.services;
using Microsoft.AspNetCore.Mvc;

namespace RunesWebScraping.controllers
{
    [ApiController]
    [Route("api/ugg")]
    public class UggRunes : Controller
    {
        private readonly UggService _uggDbService;

        public UggRunes(UggService uggService)
        {
            _uggDbService = uggService;
        }

        [HttpGet("{champion}/{lane}")]
        public async Task<IActionResult> GetRunes(string champion, string lane)
        {
            try
            {
                var championCache = await _uggDbService!.ChampionCacheExists(champion, lane);

                if (championCache == null)
                {
                    var webScrap = new RuneWebScrap(champion, lane);
                    await webScrap.GetRunes();
                    var runesId = new RunePage(webScrap.runeList.ToList());
                    var runeResponse = new RuneResponse(webScrap, runesId);
                    championCache = await _uggDbService.CreateChampionCache(runeResponse);
                }

                return Ok(championCache);
            } catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
