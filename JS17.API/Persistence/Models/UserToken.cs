namespace JS17.API.Persistence.Models;

public class UserToken : BaseModel
{
    public User User { get; set; }
    public string Token { get; set; }
}
