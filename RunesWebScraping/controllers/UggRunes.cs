using Microsoft.AspNetCore.Mvc;
using RunesWebScraping.cases;
using RunesWebScraping.controllers.interfaces;
using RunesWebScraping.domain;
using RunesWebScraping.infra;
using RunesWebScraping.repository;

namespace RunesWebScraping.controllers;

[ApiController]
[Route("api/ugg")]
public class UggRunes : Controller
{
    private readonly IUggRepository _uggDbRepository;
    private readonly IChampionsListCacheSync _championsListCacheSync;
    private readonly IRuneCacheSync _runeCacheSync;

    public UggRunes(
        IUggRepository uggRepository,
        IChampionsListCacheSync champions,
        IRuneCacheSync runeCacheSync
    )
    {
        _uggDbRepository = uggRepository;
        _championsListCacheSync = champions;
        _runeCacheSync = runeCacheSync;
    }

    [HttpGet("{champion}/{lane}")]
    public async Task<IActionResult> GetRunes(string champion, string lane)
    {
        try
        {
            var championLane = await _championsListCacheSync.GetChampionLane(champion);

            if (championLane == "not found")
                return NotFound("Champion not found.");

            lane = new LaneSanitizer(lane).NormalizedLaneName;

            if (lane == "not found")
                return BadRequest("Incorrect Lane Name.");

            var championCache = await _uggDbRepository!.ChampionCacheExists(champion, lane);

            if (championCache == null)
            {
                await _runeCacheSync.UpdateChampionCache(champion, lane);
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

            if (lane == "not found")
                return NotFound("Champion not found.");

            lane = new LaneSanitizer(lane).NormalizedLaneName;

            var championCache = await _uggDbRepository!.ChampionCacheExists(
                champion,
                lane.ToLower()
            );

            if (championCache == null)
            {
                await _runeCacheSync.UpdateChampionCache(champion, lane);
            }

            return Ok(championCache);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}
