using Microsoft.AspNetCore.Identity;
using System.Text;

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

    public int GetUserRating()
    {
        if(Posts == null) return 0;
        int rating = 0;
        foreach (var post in Posts)
        {
            rating += post.CountRating();
        }
        return rating;
    }
}
