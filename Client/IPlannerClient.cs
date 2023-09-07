using Service.Models;

namespace Client;

public interface IPlannerClient
{
    public Task<Result<Guid>> StoreTaskAsync(PlannedTask task);
    
    public Task<Result<PlannedTask>> GetTaskAsync(Guid id);

    public Task<Result> ReplaceTaskAsync(Guid id, PlannedTask task);

    public Task<Result> RemoveTaskAsync(Guid id);
    
    public Task<Result<Guid>> StoreUserAsync(PlannerUser user);
    
    public Task<Result<PlannerUser>> GetUserAsync(Guid id);

    public Task<Result> ReplaceUserAsync(Guid id, PlannerUser user);

    public Task<Result> RemoveUserAsync(Guid id);

    public Task<Result<IEnumerable<PlannedTask>>> GetUserTasksAsync(Guid id);
}