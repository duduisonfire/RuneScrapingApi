using MongoDB.Driver;
using RunesWebScraping.models;
using RunesWebScraping.controllers.classes;

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
        }

        public async Task<UggDB> CreateChampionCache(RuneResponse response)
        {
            try
            {
                var championCache = await _ugg.Find(
                    e => e.Champion!.ToLower() == response.Champion.ToLower() && e.Lane == response.Lane
                ).FirstAsync();

                if (championCache != null) return championCache!;

                UggDB newCache = new(response);
                await _ugg.InsertOneAsync(newCache);

                return newCache;
            } catch (Exception)
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
                ).FirstAsync();

                if (championCache != null) return championCache;

                return null;
            } catch (Exception)
            {
                throw;
            }
        }
    }
}
