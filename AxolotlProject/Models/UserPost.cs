namespace AxolotlProject.Models;

public class UserPost
{
    public Guid Id { get; set; }
    public DateTime CreationTime { get; }
    public string? Heading { get; set; }
    public string? Content { get; set; }
    public PostCategory PostCategory { get; set; }
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public List<Comment>? Comments { get; set; }
    public List<PostMark>? Marks { get; set; }
}
