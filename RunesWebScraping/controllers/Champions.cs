using Microsoft.AspNetCore.Mvc;
using RunesWebScraping.repository;

namespace RunesWebScraping.controllers;

[ApiController]
[Route("api/champions")]
public class Champions : Controller
{
    private readonly IChampionsRepository _championsDbRepository;

    public Champions(IChampionsRepository championsDbRepository)
    {
        _championsDbRepository = championsDbRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetRunes()
    {
        try
        {
            var championsList = await _championsDbRepository.GetAllChampions();

            return Ok(championsList);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}
