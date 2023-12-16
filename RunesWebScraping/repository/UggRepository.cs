using MongoDB.Driver;
using RunesWebScraping.models;
using RunesWebScraping.controllers.classes;
using MongoDB.Bson;
using RunesWebScraping.controllers.interfaces;

namespace RunesWebScraping.repository
{
    public class UggRepository : IUggRepository
    {
        private readonly IMongoCollection<UggDB> _ugg;

        public UggRepository()
        {
            var connectionString = Environment.GetEnvironmentVariable("CONNECTIONSTRING");
            var mongoClient = new MongoClient(connectionString);
            var mongoDatabase = mongoClient.GetDatabase("web-api");
            _ugg = mongoDatabase.GetCollection<UggDB>("UggRunesCaches");

            _ugg.Indexes.CreateOne(
                new CreateIndexModel<UggDB>(
                    new BsonDocument("createdAt", 1),
                    new CreateIndexOptions { ExpireAfter = new TimeSpan(30, 0, 0) }
                )
            );
        }

        public async Task<UggDB> CreateChampionCache(IRuneResponse response)
        {
            try
            {
                var championCache = await _ugg.Find(
                        e =>
                            e.Champion!.ToLower() == response.Champion.ToLower()
                            && e.Lane == response.Lane
                    )
                    .FirstOrDefaultAsync();

                if (championCache != null)
                    return championCache!;

                UggDB newCache = new(response);
                await _ugg.InsertOneAsync(newCache);

                return newCache;
            }
            catch (Exception)
            {
                throw new Exception("Our database is currently experiencing instabilities.");
            }
        }

        public async Task<UggDB?> ChampionCacheExists(string champion, string lane)
        {
            try
            {
                var championCache = await _ugg.Find(
                        e =>
                            string.Equals(
                                e.Champion!,
                                champion,
                                StringComparison.CurrentCultureIgnoreCase
                            )
                            && e.Lane == lane
                    )
                    .FirstOrDefaultAsync();

                return championCache ?? null;
            }
            catch (Exception)
            {
                throw new Exception("Our database is currently experiencing instabilities.");
            }
        }

        public async Task<UggDB> UpdateChampionCache(IRuneResponse response)
        {
            try
            {
                var newDocument = new UggDB(response);

                var filter = Builders<UggDB>.Filter.And(
                    Builders<UggDB>.Filter.Where(
                        e =>
                            string.Equals(
                                e.Champion,
                                response.Champion,
                                StringComparison.CurrentCultureIgnoreCase
                            )
                    ),
                    Builders<UggDB>.Filter.Where(e => e.Lane == response.Lane)
                );

                var update = Builders<UggDB>.Update
                    .Set(e => e.RunesId, response.RunesId)
                    .Set(e => e.Runes, response.Runes)
                    .Set(e => e.CreatedAt, DateTime.Now);

                var championCache = await _ugg.FindOneAndUpdateAsync<UggDB>(filter, update);

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
