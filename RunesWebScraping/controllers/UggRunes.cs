using Microsoft.AspNetCore.Mvc;
using RunesWebScraping.application;
using RunesWebScraping.controllers.classes;
using RunesWebScraping.domain;
using RunesWebScraping.models;
using RunesWebScraping.services;

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

        [HttpGet("{champion:string}/{lane:string}")]
        public async Task<ActionResult<UggDB>> GetRunes(string champion, string lane)
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
        }
    }
}
