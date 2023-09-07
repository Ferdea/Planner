using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using ThirdParty.Json.LitJson;

namespace Service.Models;

public class PlannedTask
{
    [BsonId]
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    [BsonIgnoreIfNull]
    [JsonPropertyName("description")]
    public string? Description { get; set; }
    
    [JsonPropertyName("priority")]
    public int Priority { get; set; }
    
    [JsonPropertyName("difficulty")]
    public int Difficulty { get; set; }
    
    [JsonPropertyName("execution_date")]
    public DateOnly ExecutionDate { get; set; }
    
    [JsonPropertyName("creation_date")]
    public DateTime CreationTime { get; set; }
    
    [BsonIgnoreIfNull]
    [JsonPropertyName("tag")]
    public string? Tag { get; set; }
}