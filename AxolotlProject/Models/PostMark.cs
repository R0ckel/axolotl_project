namespace AxolotlProject.Models;

public class PostMark
{
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public Guid PostId { get; set; }
    public UserPost? Post { get; set; }
    public bool Liked { get; set; }
}
