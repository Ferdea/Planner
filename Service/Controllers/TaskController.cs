using Microsoft.AspNetCore.Mvc;
using Service.Models;
using Service.Stores;

namespace Service.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class TaskController : ControllerBase
{
    private readonly IPlannedTaskStore _store;
    private readonly ILogger<TaskController> _logger;

    public TaskController(
        IPlannedTaskStore store,
        ILogger<TaskController> logger)
    {
        _store = store;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> Store([FromBody] PlannedTask task)
    {
        _logger.LogInformation($"Write task with id: {task.Id}");

        await _store.StoreAsync(task);
        _logger.LogInformation("Writing is successful!");
        
        return new JsonResult(task.Id);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute(Name = "id")]Guid id)
    {
        _logger.LogInformation($"Getting task with id: {id}");
        var task = await _store.GetAsync(id);

        if (task is null)
        {
            _logger.LogInformation("Task not found!");
            return NotFound("Task not found!");
        }

        _logger.LogInformation($"Getting is successful! Task with name: {task.Name}");
        return new JsonResult(task);
    }
    
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery(Name = "skip")] int skip, [FromQuery(Name = "take")] int take)
    {
        var tasks = await _store.GetTasksAsync(skip, take);
        
        return new JsonResult(tasks);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> Replace([FromRoute(Name = "id")]Guid id, [FromBody] PlannedTask task)
    {
        _logger.LogInformation($"Replacing task with id: {id}");
        await _store.ReplaceAsync(id, task);

        _logger.LogInformation($"Replacing is successful!");
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute(Name = "id")] Guid id)
    {
        _logger.LogInformation($"Deleting task with id: {id}");
        await _store.RemoveAsync(id);

        _logger.LogInformation("Deleting is successful!");
        return Ok();
    }
}