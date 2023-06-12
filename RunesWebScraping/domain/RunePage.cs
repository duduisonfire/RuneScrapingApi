using MongoDB.Bson.Serialization.Attributes;
using RunesWebScraping.domain.interfaces;

namespace RunesWebScraping.domain
{
    public class RunePage : IRunePage
    {
        [BsonElement("primaryStyleId")]
        public int primaryStyleId { get; }

        [BsonElement("subStyleId")]
        public int subStyleId { get; }

        [BsonElement("selectedPerkIds")]
        public int[] selectedPerkIds { get; } = {};

        public RunePage(List<string> runes)
        {
            primaryStyleId = RunesParseTable.table[runes[0]];
            subStyleId = RunesParseTable.table[runes[1]];

            for (int i = 2; i < 11; i++)
            {
                selectedPerkIds.Append(RunesParseTable.table[runes[i]]);
            }
        }
    }
}
