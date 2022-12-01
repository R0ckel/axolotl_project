namespace AxolotlProject.Models;

//public record Ban(Guid UserId, Guid BannerId, PostCategory? Category);

public class Ban
{
    public string UserId { get; set; }
    public string BannerId { get; set; }
    public PostCategory? Category { get; set; }
    public DateTime BanDate { get; set; } = DateTime.Now;

    public Ban(string userId, string bannerId, PostCategory? category)
    {
        UserId = userId;
        BannerId = bannerId;
        Category = category;
    }
}
