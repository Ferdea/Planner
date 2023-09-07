using System.Net.Http.Json;
using Service.Models;

namespace Client;

public class PlannerClient : IPlannerClient
{
    private HttpClient client;
    private string taskUrl = "https://localhost:44348/api/v1/task";
    private string userUrl = "https://localhost:44348/api/v1/user";

    public PlannerClient()
    {
        client = new HttpClient();
    }
    
    public async Task<Result<Guid>> StoreTaskAsync(PlannedTask task)
    {
        var result = await client.PostAsJsonAsync($"{taskUrl}", task);

        if (result.StatusCode.IsErrorStatusCode())
            return Result.Error<Guid>(result.StatusCode);

        var guid = await result
            .EnsureSuccessStatusCode()
            .Content
            .ReadFromJsonAsync<Guid>();

        return Result.Success(guid);
    }

    public async Task<Result<PlannedTask>> GetTaskAsync(Guid id)
    {
        var result = await client.GetAsync($"{taskUrl}/{id}");

        if (result.StatusCode.IsErrorStatusCode())
            return Result.Error<PlannedTask>(result.StatusCode);

        var task = await result
            .Content
            .ReadFromJsonAsync<PlannedTask?>();

        return Result.Build(result.StatusCode, task!);
    }

    public async Task<Result<IEnumerable<PlannedTask>>> GetTasksAsync(int skip, int take)
    {
        var url = $"{taskUrl}?skip={skip}&take={take}";

        var result = await client.GetAsync(url);

        if (result.StatusCode.IsErrorStatusCode())
            return Result.Error<IEnumerable<PlannedTask>>(result.StatusCode);

        var task = await result
            .Content
            .ReadFromJsonAsync<IEnumerable<PlannedTask>>();

        return Result.Build(result.StatusCode, task!);
    }

    public async Task<Result> ReplaceTaskAsync(Guid id, PlannedTask task)
    {
        var result = await client.PutAsJsonAsync($"{taskUrl}/{id}", task);

        return Result.Build(result.StatusCode);
    }

    public async Task<Result> RemoveTaskAsync(Guid id)
    {
        var result = await client.DeleteAsync($"{taskUrl}/{id}");

        return Result.Build(result.StatusCode);
    }

    public async Task<Result<Guid>> StoreUserAsync(PlannerUser user)
    {
        var result = await client.PostAsJsonAsync($"{userUrl}", user);

        if (result.StatusCode.IsErrorStatusCode())
            return Result.Error<Guid>(result.StatusCode);

        var guid = await result
            .EnsureSuccessStatusCode()
            .Content
            .ReadFromJsonAsync<Guid>();

        return Result.Success(guid);
    }

    public async Task<Result<PlannerUser>> GetUserAsync(Guid id)
    {
        var result = await client.GetAsync($"{taskUrl}/{id}");

        if (result.StatusCode.IsErrorStatusCode())
            return Result.Error<PlannerUser>(result.StatusCode);
        
        var user = await result
            .Content
            .ReadFromJsonAsync<PlannerUser>();

        return Result.Build(result.StatusCode, user!);
    }

    public async Task<Result> ReplaceUserAsync(Guid id, PlannerUser user)
    {
        var result = await client.PutAsJsonAsync($"{userUrl}/{id}", user);

        return Result.Build(result.StatusCode);
    }

    public async Task<Result> RemoveUserAsync(Guid id)
    {
        var result = await client.DeleteAsync($"{userUrl}/{id}");

        return Result.Build(result.StatusCode);
    }

    public async Task<Result<IEnumerable<PlannedTask>>> GetUserTasksAsync(Guid id)
    {
        var result = await client.GetAsync($"{taskUrl}/{id}/tasks");

        if (result.StatusCode.IsErrorStatusCode())
            return Result.Error<IEnumerable<PlannedTask>>(result.StatusCode);
        
        var user = await result
            .Content
            .ReadFromJsonAsync<IEnumerable<PlannedTask>>();

        return Result.Build(result.StatusCode, user!);
    }
}