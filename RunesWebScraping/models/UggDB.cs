using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using RunesWebScraping.controllers.classes;
using RunesWebScraping.domain;

namespace RunesWebScraping.models
{
    [BsonIgnoreExtraElements]
    public class UggDB
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("champion")]
        public string Champion { get; set; }

        [BsonElement("lane")]
        public string Lane { get; set; }

        [BsonElement("runes")]
        public List<string> Runes { get; set; }

        [BsonElement("runesId")]
        public RunePage RunesId { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public UggDB(RuneResponse response)
        {
            Champion = response.Champion;
            Lane = response.Lane;
            Runes = response.Runes;
            RunesId = response.RunesId;
        }
    }
}
