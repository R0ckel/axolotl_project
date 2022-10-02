namespace AxolotlProject.Models
{
    public class PostMark
    {
        public Guid UserId { get; set; }
        public Guid PostId { get; set; }
        public bool Liked { get; set; }

        //constructor
        public PostMark(Guid userId, Guid postId, bool liked)
        {
            UserId = userId;
            PostId = postId;
            Liked = liked;
        }
    }
}
