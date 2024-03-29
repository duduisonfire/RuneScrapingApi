namespace RunesWebScraping.infra;

public class LaneSanitizer
{
    public string NormalizedLaneName = "not found";

    public LaneSanitizer(string lane)
    {
        if (lane.ToLower() == "middle" || lane.ToLower() == "mid")
            NormalizedLaneName = "mid";
        if (lane.ToLower() == "bottom" || lane.ToLower() == "bot" || lane.ToLower() == "adc")
            NormalizedLaneName = "adc";
        if (lane.ToLower() == "utility" || lane.ToLower() == "supp" || lane.ToLower() == "support")
            NormalizedLaneName = "supp";
        if (lane.ToLower() == "jg" || lane.ToLower() == "jung" || lane.ToLower() == "jungle")
            NormalizedLaneName = "jungle";
        if (lane.ToLower() == "top" || lane.ToLower() == "solo")
            NormalizedLaneName = "top";
    }
}
