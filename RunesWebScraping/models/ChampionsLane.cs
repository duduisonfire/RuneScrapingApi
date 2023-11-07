using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RunesWebScraping.models
{
    [BsonIgnoreExtraElements]
    public class ChampionsLane
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("champion")]
        public string Champion { get; set; }

        [BsonElement("lane")]
        public string Lane { get; set; }

        public ChampionsLane(string champion, string lane)
        {
            Champion = champion;
            Lane = lane;
        }
    }
}
