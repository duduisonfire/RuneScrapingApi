using AngleSharp;

namespace RunesWebScraping.application
{
    public class RuneWebScrap
    {
        public readonly List<string> runeList = new();
        public readonly string champion;
        public readonly string lane;

        public RuneWebScrap(string champion, string lane)
        {
            this.champion = champion;
            this.lane = lane;
        }

        private async Task<string> ScrapRunePage()
        {
            try
            {
                var url = $"https://u.gg/lol/champions/{champion}/build/{lane}?rank=overall";
                var httpClient = new HttpClient();
                var myRequest = await httpClient.GetAsync(url);
                myRequest.EnsureSuccessStatusCode();
                var content = await myRequest.Content.ReadAsStringAsync();

                return content;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return e.Message;
            }
        }

        private async Task<List<List<string>>> SelectRunesFromPage()
        {
            try
            {
                string content = await ScrapRunePage();
                var context = BrowsingContext.New(Configuration.Default);
                var document = await context.OpenAsync(req => req.Content(content));

                var treeNameList = document
                    .QuerySelectorAll(".perk-style-title")
                    .Select(e => e.InnerHtml)
                    .ToList();

                var majorRuneList = document
                    .QuerySelectorAll(".perk-active")
                    .Select(e => e.FirstElementChild!.GetAttribute("alt")!)
                    .ToList();

                var minorsRuneList = document
                    .QuerySelectorAll(".shard-active")
                    .Select(e => e.FirstElementChild!.GetAttribute("alt")!)
                    .ToList();

                List<List<string>> runesLists =
                    new() { treeNameList, majorRuneList, minorsRuneList };
                return runesLists;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw new Exception(
                    "You didn't pass the correct parameters. Please choose a champion and a valid lane."
                );
            }
        }

        public async Task GetRunes()
        {
            try
            {
                var runesLists = await SelectRunesFromPage();

                for (int i = 0; i < 2; i++)
                {
                    runeList.Add(runesLists[0][i]);
                }

                for (int i = 0; i < 6; i++)
                {
                    var runeName = runesLists[1][i];

                    if (runeName.StartsWith("The Keystone ") || runeName.StartsWith("The Rune "))
                    {
                        runeName = runeName.Replace("The Keystone ", "");
                        runeName = runeName.Replace("The Rune ", "");
                    }

                    runeList.Add(runeName);
                }

                for (int i = 0; i < 3; i++)
                {
                    var runeName = runesLists[2][i];

                    runeName = runeName.Replace("Scaling Bonus ", "");
                    runeName = runeName.Replace("The ", "");
                    runeName = runeName.Replace(" Shard", "");

                    runeList.Add(runeName);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
    }
}
