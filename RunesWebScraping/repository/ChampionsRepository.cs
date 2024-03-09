using MongoDB.Bson;
using MongoDB.Driver;
using RunesWebScraping.models;

namespace RunesWebScraping.repository;

public class ChampionsRepository : IChampionsRepository
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

    public async Task<List<ChampionsLane>> GetAllChampions()
    {
        try
        {
            var allChampions = await _championsLane.Find(e => true).ToListAsync();

            return allChampions;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw new Exception("Our database is currently experiencing instabilities.");
        }
    }
}
