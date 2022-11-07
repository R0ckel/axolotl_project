using System;

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
    //public List<string>? Tags { get; set; }  //add in DB context

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

    public string GetShortContent(int size=500)
    {
        if (!string.IsNullOrEmpty(Content))
        {
            if (Content.Length > size)
            {
                return string.Concat(Content.AsSpan(0, size), "...");
            }
            return Content;
        }
        return "No content";
    }

    public Comment? GetLastComment()
    {
        if (Comments == null || Comments.Count == 0) return null;
        return Comments[Comments.Count - 1];
    }
}
