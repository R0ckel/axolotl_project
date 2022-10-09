namespace AxolotlProject.Models;

public class CommentMark
{
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public Guid CommentId { get; set; }
    public Comment? Comment { get; set; }
    public bool Liked { get; set; }
}
