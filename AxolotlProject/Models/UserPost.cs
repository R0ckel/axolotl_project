using Microsoft.Extensions.Hosting;

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

    //testing
    public void AddComment(Comment comment)
    {
        Comments?.Add(comment);
    }
    public void DeleteComment(Guid commentId)
    {
        Comments?.RemoveAll(p => p.Id == commentId);
    }
    public void EditComment(Comment comment)
    {
        int pos = Comments?.FindIndex(p => p.Id == comment.Id) ?? -1;
        if (pos != -1 && Comments != null)
        {
            Comments[pos] = comment;
            return;
        }
        if (pos == -1)
        {
            throw new KeyNotFoundException(nameof(comment));
        }
    }
    
    public Comment? GetComment(Guid id)
    {
        return Comments?.Find(p => p.Id == id) ?? null;
    }
}
