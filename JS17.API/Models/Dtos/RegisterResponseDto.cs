using Newtonsoft.Json;

namespace JS17.API.Models.Dtos;

public class RegisterResponseDto
{
    [JsonProperty("token")] public string Token { get; set; }
}
