using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace Service.Models;

public class PlannerUser
{
    [BsonId] 
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    
    [JsonPropertyName("task_ids")]
    public Guid[] TaskIds { get; set; }

    public PlannerUser(IEnumerable<Guid> taskIds)
    {
        TaskIds = taskIds.ToArray();
    }
}