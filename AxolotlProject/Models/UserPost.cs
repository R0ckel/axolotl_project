namespace AxolotlProject.Models;

public class UserPost
{
    public Guid Id { get; set; }
    public DateTime CreationTime { get; }
    public string? Heading { get; set; }
    public string? Content { get; set; }
    public PostCategory PostCategory { get; set; }
    public string? UserId { get; set; }
    public User? User { get; set; }
    public List<Comment>? Comments { get; set; }
    public List<PostMark>? Marks { get; set; }

    public int CountRating()
    {
        int count = 0;
        if (Marks == null)
        {
            return 0;
        }
        foreach (var mark in Marks)
        {
            if (mark.Liked) count++;
            else count--;
        }
        return count;
    }
}
