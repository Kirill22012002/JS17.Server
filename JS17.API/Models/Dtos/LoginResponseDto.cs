using Newtonsoft.Json;

namespace JS17.API.Models.Dtos;

public class LoginResponseDto
{
    [JsonProperty("token")] public string Token { get; set; }
}
