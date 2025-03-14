using Newtonsoft.Json;

namespace JS17.API.Models.Dtos;

public class UserDto
{
    [JsonProperty("id")] public int Id { get; set; }
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("email")] public string Email { get; set; }
}
