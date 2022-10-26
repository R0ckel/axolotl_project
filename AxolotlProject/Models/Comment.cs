using Microsoft.Extensions.Hosting;

namespace AxolotlProject.Models;
/// <summary>
/// Commentary Model class
/// For users` comments for posts
/// </summary>
public class Comment
{
    /// <summary>
    /// Comment`s Id
    /// </summary>
    /// <value>
    /// Generated object Id
    /// </value>
    public Guid Id { get; set; }
    /// <summary>
    /// Id of user, who created this comment
    /// </summary>
    /// <value>
    /// User`s id value
    /// </value>
    public string? UserId { get; set; }
    /// <summary>
    /// the user, who created this comment
    /// </summary>
    /// <value>
    /// Reference to User object
    /// </value>
    public User? User { get; set; }
    /// <summary>
    /// Id of post, below which commentary exists
    /// </summary>
    /// <value>
    /// Post`s id value
    /// </value>
    public Guid? PostId { get; set; }
    /// <summary>
    /// post, below which a specific comment exists
    /// </summary>
    /// <value>
    /// Reference to Post object
    /// </value>
    public UserPost? Post { get; set; }
    /// <summary>
    /// Text of this commentary
    /// </summary>
    /// <value>
    /// String value of content
    /// </value>
    public string? Content { get; set; }
    /// <summary>
    /// Points given for a specific comment
    /// </summary>
    /// <value>
    /// List of Mark-objects
    /// </value>
    public List<CommentMark>? CommentMarks { get; set; }

    /// <summary>
    /// Deletes comment from users list of comments 
    /// and from comments done for its post
    /// </summary>
    /// <returns>
    /// Status of deletion -> 
    /// true - if successfull; 
    /// false - if an error occured.
    /// </returns>
    /// <example>
    /// <code>
    /// User user = new User() { 
    ///    Comments = new List&#8249;Comment&#8250;(),
    ///    Id = "userId1"        
    /// };
    /// UserPost post = new UserPost()
    /// {
    ///    Comments = new List&#8249;Comment&#8250;(),
    ///    Id = Guid.NewGuid()
    /// };
    /// Comment comment = new Comment()
    /// {
    ///    Id = Guid.NewGuid(),
    ///    UserId = user.Id,
    ///    User = user,
    ///    PostId = post.Id,
    ///    Post = post,
    ///    Content = "Content string"
    /// };
    /// ...
    /// Console.WriteLine(user.Comments.Count());
    /// Console.WriteLine(post.Comments.Count());
    /// Console.WriteLine(user.Comments.First().Content == comment.Content);
    /// comment.DeleteSelf();
    /// Console.WriteLine(user.Comments.Count());
    /// Console.WriteLine(post.Comments.Count());
    /// </code>
    /// </example>
    public bool DeleteSelf()
    {
        try
        {
            User?.Comments?.Remove(this);
            Post?.Comments?.Remove(this);
        } catch (Exception){
            return false;
        }
        return true;
        //deletion
    }
}
