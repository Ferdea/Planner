using Microsoft.AspNetCore.Mvc;
using Service.Models;
using Service.Stores;

namespace Service.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class UserController : ControllerBase
{
    private IPlannerUserStore store;
    private ILogger<UserController> logger;

    public UserController(
        IPlannerUserStore store,
        ILogger<UserController> logger)
    {
        this.store = store;
        this.logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> Store([FromBody] PlannerUser user)
    {
        logger.LogInformation($"Write user");
        
        var id = Guid.NewGuid();
        logger.LogInformation($"User id: {id}");
        
        await store.StoreAsync(id, user);
        logger.LogInformation("Writing is successful!");
        
        return new JsonResult(id);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute(Name = "id")]Guid id)
    {
        logger.LogInformation($"Getting user with id: {id}");
        var user = await store.GetAsync(id);

        if (user is null)
        {
            logger.LogInformation("User not found!");
            return NotFound("User not found!");
        }

        logger.LogInformation($"Getting is successful!");
        return new JsonResult(user);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> Replace([FromRoute(Name = "id")]Guid id, [FromBody] PlannerUser user)
    {
        logger.LogInformation($"Replacing user with id: {id}");
        await store.ReplaceAsync(id, user);

        logger.LogInformation($"Replacing is successful!");
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute(Name = "id")] Guid id)
    {
        logger.LogInformation($"Deleting user with id: {id}");
        await store.RemoveAsync(id);

        logger.LogInformation("Deleting is successful!");
        return Ok();
    }
    
    [HttpGet("{id}/tasks")]
    public async Task<IActionResult> GetTasks([FromRoute(Name = "id")]Guid id)
    {
        logger.LogInformation($"Getting user tasks with id: {id}");
        var tasks = await store.GetTasks(id);

        if (!tasks.Any())
        {
            logger.LogInformation("User tasks not found!");
            return NotFound("User tasks not found!");
        }

        logger.LogInformation($"Getting is successful!");
        return new JsonResult(tasks);
    }
}