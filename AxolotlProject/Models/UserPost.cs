using System.Text.RegularExpressions;

namespace AxolotlProject.Models;

public class UserPost
{
    public Guid Id { get; set; }
    public DateTime CreationTime { get; set; } = DateTime.Now;
    public string? Heading { get; set; }
    public string? Content { get; set; }
    public PostCategory PostCategory { get; set; }
    public string? UserId { get; set; }
    public User? User { get; set; }
    public List<Comment>? Comments { get; set; }
    public List<PostMark>? Marks { get; set; }
    public IEnumerable<string>? Tags
    {
        get
        {
            Regex r = new Regex(@"\#.\w*", RegexOptions.IgnoreCase);
            List<string> matches = new();
            if (Heading is null || Content is null)
                throw new ArgumentNullException();
            return matches
                            .Concat(r.Matches(Heading).Cast<Match>().Select(match => match.Value))
                            .Concat(r.Matches(Content).Cast<Match>().Select(match => match.Value))
                                                        .Distinct();
        }
    }

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

    public string GetShortContent(int size = 500)
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
