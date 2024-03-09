using MongoDB.Bson.Serialization.Attributes;
using RunesWebScraping.domain;

namespace RunesWebScraping.domain
{
    public class RunePage
    {
        [BsonElement("runePageTitle")]
        public string RunePageTitle { get; set; }

        [BsonElement("primaryStyleId")]
        public int PrimaryStyleId { get; set; }

        [BsonElement("subStyleId")]
        public int SubStyleId { get; set; }

        [BsonElement("selectedPerkIds")]
        public List<int> SelectedPerkIds { get; set; }

        public RunePage(List<string> runes)
        {
            RunePageTitle = runes[^1];
            SelectedPerkIds = new();
            PrimaryStyleId = RunesParseTable.table[runes[0]];
            SubStyleId = RunesParseTable.table[runes[1]];

            for (int i = 2; i < 11; i++)
            {
                SelectedPerkIds.Add(RunesParseTable.table[runes[i]]);
            }
        }
    }
}
