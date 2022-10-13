namespace AxolotlProject.Models;

public class Comment
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public Guid PostId { get; set; }
    public UserPost? Post { get; set; }
    public string? Content { get; set; }
    public List<CommentMark>? CommentMarks { get; set; }
}
