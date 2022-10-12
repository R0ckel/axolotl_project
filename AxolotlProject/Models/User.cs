using Microsoft.AspNetCore.Identity;

namespace AxolotlProject.Models;

public class User : IdentityUser
{
    [PersonalData]
    public string? Login { get; set; }

    [PersonalData]
    public string? UserSurname { get; set; }

    [PersonalData]
    public string? Status { get; set; }

    [PersonalData]
    public bool IsBanned { get; set; }

    // [PersonalData]
    // public IFormFile? Icon { get; set; } //need to save user`s icon. File - static class.
    
    [PersonalData]
    public DateTime? BirthDate { get; set; }

    [PersonalData]
    public UserSex? Sex { get; set; }

    [PersonalData]
    public string? Description { get; set; } = "no description";

    [PersonalData]
    public List<UserPost>? Posts { get; set; }

    [PersonalData]
    public List<PostMark>? PostMarks { get; set; }

    [PersonalData]
    public List<Comment>? Comments { get; set; }

    [PersonalData]
    public List<CommentMark>? CommentMarks { get; set; }

    //testing
    public void AddPost(UserPost userPost)
    {
        Posts?.Add(userPost);
    }
    public void DeletePost(Guid postId)
    {
        Posts?.RemoveAll(p => p.Id == postId);
    }
    public void EditPost(UserPost userPost)
    {
        int pos = Posts?.FindIndex(p => p.Id == userPost.Id)??-1;
        if (pos != -1 && Posts != null)
        {
            Posts[pos] = userPost;
            return;
        }
        if (pos == -1)
        {
            throw new KeyNotFoundException(nameof(userPost));
        }
    }

    public UserPost? GetPost(Guid id)
    {
        return Posts?.Find(p => p.Id == id) ?? null;
    }
}
