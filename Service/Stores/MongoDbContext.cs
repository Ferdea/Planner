using MongoDB.Driver;

namespace Service.Stores;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(PlannerSettings settings)
    {
        var client = new MongoClient(settings.ConnectionString);
        _database = client.GetDatabase(settings.Database);
    }

    public IMongoCollection<T> GetCollection<T>(string collectionName)
    {
        return _database.GetCollection<T>(collectionName);
    }
}