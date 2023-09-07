using FluentAssertions;
using Service.Models;

namespace ClientTests;

public class CrudTaskTests
{
    private PlannerClient client;

    private const string ForTestTag = "for test";
    
    [OneTimeSetUp]
    public void Setup()
    {
        client = new PlannerClient();
    }

    [OneTimeTearDown]
    public async Task TearDown()
    {
        var take = 1;
        var skip = 0;
        
        while (true)
        {
            var result = await client.GetTasksAsync(skip, take);

            if (result.IsErrorStatus || result.Value is null)
                break;
            
            var tasks = result.Value.ToArray();

            if (tasks.Length == 0)
                break;

            skip += tasks.Count(task => task.Tag != ForTestTag);

            foreach (var task in tasks.Where(task => task.Tag == ForTestTag))
                await client.RemoveTaskAsync(task.Id);
        }
    }

    [Test]
    public async Task GetTaskAsync_ShouldReturnError_WhenStoreTaskAsyncIsNotCalled()
    {
        var resultTask = await client.GetTaskAsync(Guid.Empty);

        resultTask
            .IsErrorStatus
            .Should()
            .BeTrue();

        resultTask
            .Value
            .Should()
            .BeNull();
    }

    [Test]
    public async Task GetTaskAsync_ShouldReturnPlannedTask_WhenStoreTaskAsyncIsSuccessful()
    {
        var task = new PlannedTask
        {
            Id = Guid.NewGuid(),
            Name = "test",
            Description = "test",
            Difficulty = 993,
            Priority = 993,
            ExecutionDate = DateOnly.FromDateTime(DateTime.Today),
            CreationTime = DateTime.Today.ToUniversalTime(),
            Tag = ForTestTag
        };

        var resultId = await client.StoreTaskAsync(task);

        var resultTask = await client.GetTaskAsync(resultId.Value);

        resultTask
            .IsErrorStatus
            .Should()
            .BeFalse();
        
        resultTask
            .Value
            .Should()
            .BeEquivalentTo(task);
    }

    [Test]
    public async Task GetTaskAsync_ShouldReturnNewTask_AfterReplacing()
    {
        var firstTask = new PlannedTask
        {
            Id = Guid.NewGuid(),
            Name = "test",
            Description = "test",
            Difficulty = 0,
            Priority = 0,
            ExecutionDate = DateOnly.FromDateTime(DateTime.Today),
            CreationTime = DateTime.Today.ToUniversalTime(),
            Tag = ForTestTag
        };

        var secondTask = new PlannedTask
        {
            Id = firstTask.Id,
            Name = "new",
            Description = "new",
            Difficulty = 993,
            Priority = 993,
            ExecutionDate = DateOnly.FromDateTime(DateTime.Today),
            CreationTime = DateTime.Today.ToUniversalTime(),
            Tag = ForTestTag
        };

        var resultId = await client.StoreTaskAsync(firstTask);

        await client.ReplaceTaskAsync(resultId.Value, secondTask);

        var resultTask = await client.GetTaskAsync(resultId.Value);

        resultTask
            .IsErrorStatus
            .Should()
            .BeFalse();
        
        resultTask
            .Value
            .Should()
            .BeEquivalentTo(secondTask);
    }

    [Test]
    public async Task GetTaskAsync_AfterDeleting()
    {
        var task = new PlannedTask
        {
            Id = Guid.NewGuid(),
            Name = "test",
            Description = "test",
            Difficulty = 993,
            Priority = 993,
            ExecutionDate = DateOnly.FromDateTime(DateTime.Today),
            CreationTime = DateTime.Today.ToUniversalTime(),
            Tag = ForTestTag
        };

        var resultId = await client.StoreTaskAsync(task);

        await client.RemoveTaskAsync(resultId.Value);

        var resultTask = await client.GetTaskAsync(resultId.Value);

        resultTask
            .IsErrorStatus
            .Should()
            .BeTrue();
        
        resultTask
            .Value
            .Should()
            .BeNull();
    }
}