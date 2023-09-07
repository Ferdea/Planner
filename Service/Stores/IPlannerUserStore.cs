using Service.Models;

namespace Service.Stores;

public interface IPlannerUserStore
{
    public Task StoreAsync(Guid id, PlannerUser task);

    public Task<PlannerUser> GetAsync(Guid id);

    public Task ReplaceAsync(Guid id, PlannerUser newTask);

    public Task RemoveAsync(Guid id);

    public Task<IEnumerable<PlannedTask>> GetTasks(Guid id);
}