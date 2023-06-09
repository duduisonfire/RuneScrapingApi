using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace RunesWebScraping.controllers
{
    [ApiController]
    [Route("api/ugg")]
    public class UggRunes : Controller
    {
        [HttpGet]
        public string GetRunes()
        {
            var testString = "Essa string é um teste.";

            return testString;
        }
    }
}