namespace AxolotlProject.Models
{
    public class Comment
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid PostId { get; set; }
        string Content { get; set; }

        //constructor
        public Comment(Guid id, Guid userId, Guid postId, string content)
        {
            Id = id;
            UserId = userId;
            PostId = postId;
            Content = content;
        }
    }
}
