using RunesWebScraping.models;
using MongoDB.Driver;
using MongoDB.Bson;

namespace RunesWebScraping.repository;

public class ChampionsRepository
{
    private readonly IMongoCollection<ChampionsLane> _championsLane;

    public ChampionsRepository()
    {
        var connectionString = Environment.GetEnvironmentVariable("CONNECTIONSTRING");
        var mongoClient = new MongoClient(connectionString);
        var mongoDatabase = mongoClient.GetDatabase("web-api");
        _championsLane = mongoDatabase.GetCollection<ChampionsLane>("ChampionsLane");
    }

    public async Task<ChampionsLane> CreateChampionLaneCache(string champion, string lane)
    {
        try
        {
            var championCache = await _championsLane
                .Find(e => e.Champion!.ToLower() == champion.ToLower())
                .FirstOrDefaultAsync();

            if (championCache != null)
                return championCache!;

            ChampionsLane newCache = new(champion, lane);
            await _championsLane.InsertOneAsync(newCache);

            return newCache;
        }
        catch (Exception)
        {
            throw new Exception("Our database is currently experiencing instabilities.");
        }
    }

    public async Task<ChampionsLane?> GetChampionLane(string champion)
    {
        try
        {
            var championCache = await _championsLane
                .Find(e => e.Champion!.ToLower() == champion.ToLower())
                .FirstOrDefaultAsync();

            return championCache ?? null;
        }
        catch (Exception)
        {
            throw new Exception("Our database is currently experiencing instabilities.");
        }
    }

    public async Task<long> ChampionsLaneCacheLength()
    {
        try
        {
            var length = await _championsLane.CountDocumentsAsync(new BsonDocument());

            return length;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw new Exception("Our database is currently experiencing instabilities.");
        }
    }
}