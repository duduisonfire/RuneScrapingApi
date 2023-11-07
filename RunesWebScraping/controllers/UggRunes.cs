using RunesWebScraping.cases;
using RunesWebScraping.controllers.classes;
using RunesWebScraping.domain;
using RunesWebScraping.repository;
using Microsoft.AspNetCore.Mvc;
using RunesWebScraping.infra;

namespace RunesWebScraping.controllers;

[ApiController]
[Route("api/ugg")]
public class UggRunes : Controller
{
    private readonly UggRepository _uggDbRepository;
    private readonly ChampionsListCacheSync _championsListCacheSync;

    public UggRunes(UggRepository uggRepository, ChampionsListCacheSync champions)
    {
        _uggDbRepository = uggRepository;
        _championsListCacheSync = champions;
    }

    [HttpGet("{champion}/{lane}")]
    public async Task<IActionResult> GetRunes(string champion, string lane)
    {
        try
        {
            var championLane = await _championsListCacheSync.GetChampionLane(champion);
            lane = new LaneSanitizer(lane).NormalizedLaneName;

            if (championLane == "not found")
                return NotFound("Champion not found.");
            if (lane == "not found")
                lane = championLane;

            var championCache = await _uggDbRepository!.ChampionCacheExists(champion, lane);

            if (championCache == null)
            {
                var webScrap = new UggWebScrap(champion, lane);
                var runes = await webScrap.GetRunes();
                var pageBuilder = new RunesPageBuilder(runes, champion, lane);
                var runesId = new List<RunePage>();

                for (int i = 0; i < pageBuilder.listOfRunesId.Count; i++)
                {
                    runesId.Add(pageBuilder.listOfRunesId[i]);
                }

                var runeResponse = new RuneResponse(pageBuilder);
                championCache = await _uggDbRepository.CreateChampionCache(runeResponse);
            }

            return Ok(championCache);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("{champion}")]
    public async Task<IActionResult> GetRunesWithoutLane(string champion)
    {
        try
        {
            var lane = await _championsListCacheSync.GetChampionLane(champion);

            var championCache = await _uggDbRepository!.ChampionCacheExists(
                champion,
                lane.ToLower()
            );

            if (championCache == null)
            {
                var webScrap = new UggWebScrap(champion, lane);
                var runes = await webScrap.GetRunes();
                var pageBuilder = new RunesPageBuilder(runes, champion, lane);
                var runesId = new List<RunePage>();

                for (int i = 0; i < pageBuilder.listOfRunesId.Count; i++)
                {
                    runesId.Add(pageBuilder.listOfRunesId[i]);
                }

                var runeResponse = new RuneResponse(pageBuilder);
                championCache = await _uggDbRepository.CreateChampionCache(runeResponse);
            }

            return Ok(championCache);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}
