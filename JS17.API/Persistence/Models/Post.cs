namespace JS17.API.Persistence.Models;

public class Post : BaseModel
{
    public string Title { get; set; }
    public string Text { get; set; }
    public bool IsRemoved { get; set; } = false;
    public User Owner { get; set; }
}
