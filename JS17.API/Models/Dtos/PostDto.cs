using Newtonsoft.Json;

namespace JS17.API.Models.Dtos;

public class PostDto
{
    [JsonProperty("id")] public int Id { get; set; }
    [JsonProperty("title")] public string Title { get; set; }
    [JsonProperty("text")] public string Text { get; set; }
}
