using Service.Models;

namespace Service.Stores;

public class InMemoryPlannedTaskStore : IPlannedTaskStore
{
    private SortedDictionary<Guid, PlannedTask> store;
    private ILogger<InMemoryPlannedTaskStore> logger;

    public InMemoryPlannedTaskStore(
        ILogger<InMemoryPlannedTaskStore> logger)
    {
        this.logger = logger;
        
        store = new SortedDictionary<Guid, PlannedTask>();
    }

    public Task StoreAsync(PlannedTask task)
    {
        if (!store.TryAdd(task.Id, task))
            throw new InvalidOperationException();

        return Task.CompletedTask;
    }

    public Task<PlannedTask?> GetAsync(Guid id)
    {
        logger.LogInformation($"Getting task with id: {id}");
        if (!store.TryGetValue(id, out var task))
        {
            logger.LogInformation($"Task with id {id} not found");
            return Task.FromResult<PlannedTask?>(null);
        }

        logger.LogInformation("Getting is successful!");
        return Task.FromResult<PlannedTask?>(task);
    }

    public Task<IEnumerable<PlannedTask>> GetTasksAsync(int skip, int take)
    {
        return Task.FromResult(store
            .Select(pair => pair.Value)
            .Skip(skip)
            .Take(take));
    }

    public Task ReplaceAsync(Guid id, PlannedTask newTask)
    {
        if (newTask.Id != id)
            throw new ArgumentException();

        if (!store.ContainsKey(id))
            throw new InvalidOperationException();

        store.Remove(id);
        store.Add(id, newTask);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(Guid id)
    {
        store.Remove(id);
        return Task.CompletedTask;
    }
}