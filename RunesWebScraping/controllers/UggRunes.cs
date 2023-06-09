using Microsoft.AspNetCore.Mvc;
using RunesWebScraping.domain;

namespace RunesWebScraping.controllers
{
    [ApiController]
    [Route("api/ugg")]
    public class UggRunes : Controller
    {
        [HttpGet]
        public async Task<string> GetRunes()
        {
            var lolApi = new LolApi();

            var testString = await lolApi.GetLolVersion();

            return testString;
        }
    }
}
