namespace AxolotlProject.Models
{
    public class CommentMark
    {
        public Guid UserId { get; set; }
        public Guid CommentId { get; set; }
        public bool Liked { get; set; }

        //constructor
        public CommentMark(Guid userId, Guid commentId, bool liked)
        {
            UserId = userId;
            CommentId = commentId;
            Liked = liked;
        }
    }
}
