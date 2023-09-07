using MongoDB.Driver;
using Service.Models;

namespace Service.Stores;

public class MongoDbPlannedTaskStore : IPlannedTaskStore
{
    private readonly MongoDbContext _context;
    private readonly string _tableName = PlannerConstants.TasksTableName;
    
    public MongoDbPlannedTaskStore(
        MongoDbContext context)
    {
        _context = context;
    }
    
    public async Task StoreAsync(PlannedTask task)
    {
        await _context
            .GetCollection<PlannedTask>(_tableName)
            .InsertOneAsync(task);
    }

    public async Task<PlannedTask?> GetAsync(Guid id)
    {
        var result = await _context
            .GetCollection<PlannedTask>(_tableName)
            .FindAsync(task => task.Id == id);
        
        return result
            .FirstOrDefault();
    }

    public async Task<IEnumerable<PlannedTask>> GetTasksAsync(int skip, int take)
    {
        var result = await _context
            .GetCollection<PlannedTask>(_tableName)
            .FindAsync(_ => true, new FindOptions<PlannedTask> { Skip = skip, Limit = take });
        
        return await result.ToListAsync();
    }

    public async Task ReplaceAsync(Guid id, PlannedTask newTask)
    {
        if (newTask.Id != id)
            throw new ArgumentException();

        await _context
            .GetCollection<PlannedTask>(_tableName)
            .ReplaceOneAsync(task => task.Id == id, newTask);
    }

    public async Task RemoveAsync(Guid id)
    {
        await _context
            .GetCollection<PlannedTask>(_tableName)
            .DeleteOneAsync(task => task.Id == id);
    }
}