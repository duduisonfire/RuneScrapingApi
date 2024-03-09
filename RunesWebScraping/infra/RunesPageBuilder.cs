using RunesWebScraping.domain;

namespace RunesWebScraping.infra;

public class RunesPageBuilder
{
    public readonly string champion;
    public readonly string lane;
    public readonly List<IRunePage> listOfRunesId = new();
    public readonly HashSet<List<string>> listOfRunesName = new();

    public RunesPageBuilder(List<List<string>> runes, string champion, string lane)
    {
        this.champion = champion;
        this.lane = lane;

        for (int i = 0; i < runes.Count; i++)
        {
            List<string> runeList = new();

            for (int j = 0; j < runes[i].Count; j++)
            {
                var runeName = runes[i][j];

                if (runeName.StartsWith("The Keystone ") || runeName.StartsWith("The Rune "))
                {
                    runeName = runeName.Replace("The Keystone ", "");
                    runeName = runeName.Replace("The Rune ", "");
                }

                runeName = runeName.Replace("The ", "");
                runeName = runeName.Replace("Scaling ", "");
                runeName = runeName.Replace("Bonus ", "");
                runeName = runeName.Replace(" Shard", "");

                runeList.Add(runeName);
            }

            listOfRunesName.Add(runeList);
            IRunePage runePage = new RunePage(runeList);
            listOfRunesId.Add(runePage);
        }
    }
}
