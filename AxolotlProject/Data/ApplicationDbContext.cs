using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AxolotlProject.Models;

namespace AxolotlProject.Data;

public class ApplicationDbContext : IdentityDbContext<User, Microsoft.AspNetCore.Identity.IdentityRole, string>
{
    public DbSet<UserPost>? UserPosts { get; set; }
    public DbSet<PostMark>? PostsMarks { get; set; }
    public DbSet<Comment>? Comments { get; set; }
    public DbSet<CommentMark>? CommentsMarks { get; set; }
    public DbSet<Ban>? Bans { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserPost>()
            .HasOne(post => post.User)
            .WithMany(user => user.Posts);
        modelBuilder.Entity<UserPost>()
            .HasMany(post => post.Comments)
            .WithOne(comment => comment.Post);
        modelBuilder.Entity<UserPost>()
            .HasMany(post => post.Marks)
            .WithOne(mark => mark.Post);

        modelBuilder.Entity<Comment>()
            .HasOne(comment => comment.User)
            .WithMany(user => user.Comments);
        modelBuilder.Entity<Comment>()
            .HasMany(comment => comment.CommentMarks)
            .WithOne(mark => mark.Comment);

        modelBuilder.Entity<PostMark>()
            .HasOne(mark => mark.User)
            .WithMany(user => user.PostMarks);
        modelBuilder.Entity<PostMark>()
            .HasKey(mark => new { mark.UserId, mark.PostId });

        modelBuilder.Entity<CommentMark>()
            .HasOne(mark => mark.User)
            .WithMany(user => user.CommentMarks);
        modelBuilder.Entity<CommentMark>()
            .HasKey(mark => new { mark.UserId, mark.CommentId });

        modelBuilder.Entity<Ban>()
                .HasKey(ban => new { ban.UserId, ban.Category });

        base.OnModelCreating(modelBuilder);
    }

    public bool IsUserBanned(string id, PostCategory category) {
        return Bans!.Any(ban => ban.UserId.Equals(id) && ban.Category.Equals(category));
    }
}
