using MongoDB.Driver;
using RunesWebScraping.models;
using RunesWebScraping.controllers.classes;
using MongoDB.Bson;

namespace RunesWebScraping.services
{
    public class UggService
    {
        private readonly IMongoCollection<UggDB> _ugg;

        public UggService()
        {
            var connectionString = Environment.GetEnvironmentVariable("CONNECTIONSTRING");
            var mongoClient = new MongoClient(connectionString);
            var mongoDatabase = mongoClient.GetDatabase("web-api");
            _ugg = mongoDatabase.GetCollection<UggDB>("uggrunescaches");
            // var index = new IndexKeysDefinitionBuilder<UggDB>().Ascending(e => e.CreatedAt);
            // _ugg.Indexes.CreateOne(
            //     new CreateIndexModel<UggDB>(
            //         index,
            //         new CreateIndexOptions { ExpireAfter = TimeSpan.FromSeconds(60 * 60 * 30) }
            //     )
            // );
        }

        public async Task<UggDB> CreateChampionCache(RuneResponse response)
        {
            try
            {
                var championCache = await _ugg.Find(
                        e =>
                            e.Champion!.ToLower() == response.Champion.ToLower()
                            && e.Lane == response.Lane
                    )
                    .FirstAsync();

                if (championCache != null)
                    return championCache!;

                UggDB newCache = new(response);
                await _ugg.InsertOneAsync(newCache);

                return newCache;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UggDB?> ChampionCacheExists(string champion, string lane)
        {
            try
            {
                var championCache = await _ugg.Find(
                        e => e.Champion!.ToLower() == champion.ToLower() && e.Lane == lane
                    )
                    .FirstAsync();

                if (championCache != null)
                    return championCache;

                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UggDB> UpdateChampionCache(RuneResponse response)
        {
            try
            {
                var newDocument = new UggDB(response);

                var filter = Builders<UggDB>.Filter.And(
                    Builders<UggDB>.Filter.Where(
                        e => e.Champion.ToLower() == response.Champion.ToLower()
                    ),
                    Builders<UggDB>.Filter.Where(e => e.Lane == response.Lane)
                );

                var championCache = await _ugg.FindOneAndReplaceAsync<UggDB>(filter, newDocument);

                return championCache;
            }
            catch (Exception)
            {
                throw new Exception("Our database is currently experiencing instabilities.");
            }
        }

        public async Task<long> ChampionRunesCacheLength()
        {
            try
            {
                var length = await _ugg.CountDocumentsAsync(new BsonDocument());

                return length;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw new Exception("Our database is currently experiencing instabilities.");
            }
        }
    }
}
