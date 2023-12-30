namespace RunesWebScraping.infra;

public class LaneSanitizer
{
    public string NormalizedLaneName = "not found";

    public LaneSanitizer(string lane)
    {
        if (lane.ToLower() == "middle" || lane.ToLower() == "mid")
            NormalizedLaneName = "mid";
        if (lane.ToLower() == "bottom" || lane.ToLower() == "bot")
            NormalizedLaneName = "adc";
        if (lane.ToLower() == "utility")
            NormalizedLaneName = "supp";
        if (lane.ToLower() == "jg" || lane.ToLower() == "jung" || lane.ToLower() == "jungle")
            NormalizedLaneName = "jungle";
    }
}
