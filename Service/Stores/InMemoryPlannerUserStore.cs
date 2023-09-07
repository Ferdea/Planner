using Service.Models;

namespace Service.Stores;

public class InMemoryPlannerUserStore : IPlannerUserStore
{
    private Dictionary<Guid, PlannerUser> store;
    private IPlannedTaskStore taskStore;

    public InMemoryPlannerUserStore(IPlannedTaskStore taskStore)
    {
        this.taskStore = taskStore;

        store = new Dictionary<Guid, PlannerUser>();
    }
    
    public Task StoreAsync(Guid id, PlannerUser task)
    {
        if (!store.TryAdd(id, task))
            throw new InvalidOperationException();

        return Task.CompletedTask;
    }

    public Task<PlannerUser> GetAsync(Guid id)
    {
        if (!store.TryGetValue(id, out var user))
            throw new ArgumentException();

        return Task.FromResult(user);
    }

    public Task ReplaceAsync(Guid id, PlannerUser newTask)
    {
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

    public async Task<IEnumerable<PlannedTask>> GetTasks(Guid id)
    {
        var user = await GetAsync(id);

        return user.TaskIds
            .Select(id => taskStore.GetAsync(id).Result);
    }
}