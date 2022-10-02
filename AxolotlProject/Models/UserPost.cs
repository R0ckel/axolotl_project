namespace AxolotlProject.Models
{
    public class UserPost
    {
        public Guid Id { get; }
        DateTime CreationTime { get; }
        string Heading { get; set; }
        string Content { get; set; }
        PostCategory PostCategory { get; set; }

        //constructors
        public UserPost() { }
        public UserPost(Guid id, DateTime creationTime, string heading, string content, PostCategory postCategory)
        {
            Id = id;
            CreationTime = creationTime;
            Heading = heading;
            Content = content;
            PostCategory = postCategory;
        }
    }
}
