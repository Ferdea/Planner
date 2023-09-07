using Service.Models;

namespace Service.Stores;

public interface IPlannedTaskStore
{
    public Task StoreAsync(PlannedTask task);

    public Task<PlannedTask?> GetAsync(Guid id);
    
    public Task<IEnumerable<PlannedTask>> GetTasksAsync(int skip, int take);

    public Task ReplaceAsync(Guid id, PlannedTask newTask);

    public Task RemoveAsync(Guid id);
}