namespace AxolotlProject.Models;

public class Comment
{
    public Guid Id { get; set; }
    public string? UserId { get; set; }
    public User? User { get; set; }
    public Guid? PostId { get; set; }
    public UserPost? Post { get; set; }
    public string? Content { get; set; }
    public List<CommentMark>? CommentMarks { get; set; }
    public DateTime CreationTime { get; set; } = DateTime.Now;

    public void DeleteSelf()
    {
        User?.Comments?.Remove(this);
        Post?.Comments?.Remove(this);
        //deletion
    }
}
